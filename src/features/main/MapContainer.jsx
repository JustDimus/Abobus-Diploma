import { useDispatch, useSelector } from "react-redux";
import React, { useEffect } from "react";
import { jwtService } from "../../app/jwtService";
import { selectAuthorization, setAuthorization } from "../../app/slice/authorizationSlice";
import {
    selectNotification,
    setErrorMessage,
    setMessage,
} from "../../app/slice/notificationSlice";
import { notification, Row, message } from "antd";
import { useNavigate } from "react-router-dom";
import NotAuthorizedContainer from "../common/NotAuthorizedContainer";
import {
    useJsApiLoader,
    GoogleMap,
    Marker,
    Autocomplete,
    DirectionsRenderer,
} from '@react-google-maps/api'
import { useRef, useState } from 'react'
import {
    Box,
    Button,
    ButtonGroup,
    Flex,
    HStack,
    IconButton,
    Input,
    SkeletonText,
    Text,
} from '@chakra-ui/react'
import { FaBackspace, FaTimes } from 'react-icons/fa'
import { useMemo } from "react";
import {
    setRoute,
    cleanRouteData,
    deletePreviousRoute
} from "../../app/slice/map/routeSlice"
import { secondsToDhm } from "../helpers/timeHelper"

const libraries = ['places']

const MapContainer = () => {
    const [messageApi, contextHolder] = message.useMessage();

    const center = useMemo(() => ({ lat: 50.450001, lng: 30.523333 }), []);
    const options = useMemo(() => ({
        mapId: "131d27392ad7fb91",
        zoomControl: false,
        streetViewControl: false,
        mapTypeControl: false,
        fullscreenControl: false,
        disableDoubleClickZoom: true,

        disableDefaultUI: true,
        clickableIcons: true,
    }), []);

    const dispatch = useDispatch();
    const state = useSelector(selectAuthorization);
    const notifications = useSelector(selectNotification);
    const navigate = useNavigate();

    const { isLoaded } = useJsApiLoader({
        googleMapsApiKey: process.env.REACT_APP_GOOGLE_MAPS_API_KEY,
        libraries
    });

    const [map, setMap] = useState((null))
    const [directionsResponse, setDirectionsResponse] = useState(null)
    const [distanceState, setDistanceState] = useState('')
    const [durationState, setDurationState] = useState('')
    const [wayPointsState, setWayPointsState] = useState({})

    const originRef = useRef()
    const destiantionRef = useRef()
    const wayPointsRef = useRef()

    let originMarkerRef;
    let destiantionMarkerRef;
    let wayPointsMarkerRef;

    const [markers, setMarkers] = useState([]);

    const onMapClick = (e) => {
        setMarkers((current) => [
            ...current,
            {
                lat: parseFloat(e.latLng.lat()),
                lng: parseFloat(e.latLng.lng())
            }
        ]);

        console.log("markers => " + markers);
        console.log("waypoints => " + wayPointsMarkerRef);
    };

    const setMarkersRefVariables = () => {
        wayPointsMarkerRef = markers.map(p => ({
            location: { lat: p.lat, lng: p.lng },
        }));

        originMarkerRef = wayPointsMarkerRef.shift().location;
        destiantionMarkerRef = wayPointsMarkerRef.pop().location;

        if (wayPointsMarkerRef && originMarkerRef && destiantionMarkerRef) {
            calculateRouteWithMarkers();
        }
    }

    const onMarkerRightClick = (e) => {
        /* const lat = parseFloat(e.latLng.lat());
        const lng = parseFloat(e.latLng.lng()); */
        const lat = parseFloat(e.lat);
        const lng = parseFloat(e.lng);

        if (markers.length == 1) {
            setMarkers(() => []);
        }
        else {
            setMarkers((current) => {
                return current.filter(marker => marker.lat !== lat && marker.lng !== lng)
            });
        }

        console.log(markers);
    }

    const openErrorNotification = (error) => {
        if (error === "ZERO_RESULTS") {
            notification["error"]({
                message: "Can't build such a route.",
                description: notifications.errorMessage,
            });
        }

        dispatch(setErrorMessage(undefined));
    };

    const navigationClick = (e) => {
        navigate(e.key);
    }

    const openNotification = () => {
        notification["success"]({
            message: "Note",
            description: notifications.message,
        });
        dispatch(setMessage(undefined));
    }

    useEffect(() => {
        if (jwtService.get()) {
            dispatch(setAuthorization(true));
        }

        console.log("1");
    }, [dispatch]);

    useEffect(() => {
        if (markers.length > 1) {
            setMarkersRefVariables();
        }
        console.log("2 => " + markers);
    }, [markers]);

    if (!isLoaded) {
        return <SkeletonText />
    }

    function setRouteForSlice(results) {
        /* dispatch(setRoute({
            startAddress: results.request.origin.query,
            endAddress: results.request.destination.query,
            startLocation: results.routes[0].legs[0].start_location.toJSON(),
            endLocation: results.routes[0].legs[0].end_location.toJSON(),
            distance: results.routes[0].legs[0].distance.value,
            duration: results.routes[0].legs[0].duration.value,
            geocodedWaypoints: results.geocoded_waypoints,
        })) */

        dispatch(setRoute({
            results
        }))
    }

    async function calculateRouteWithMarkers() {
        // eslint-disable-next-line no-undef
        const directionsService = new google.maps.DirectionsService()
        await directionsService.route({
            origin: originMarkerRef,
            destination: destiantionMarkerRef,
            waypoints: wayPointsMarkerRef ? wayPointsMarkerRef : null,
            // eslint-disable-next-line no-undef
            travelMode: google.maps.TravelMode.DRIVING,
        }, (response, status) => {
            if (status === "OK") {
                const temp = response.routes[0].legs.length - 1;

                originRef.current.value = response.routes[0].legs[0].start_address;
                destiantionRef.current.value = response.routes[0].legs[temp].end_address;

                setRouteForSlice(response);
                setDirectionsResponse(response);

                let distanceTemp = 0;
                let durationTemp = 0;

                for (let i = 0; i < response.routes[0].legs.length; i++) {
                    distanceTemp += response.routes[0].legs[i].distance.value;
                    durationTemp += response.routes[0].legs[i].duration.value;
                }

                distanceTemp = (distanceTemp / 1000).toFixed(1);
                let durationTempString = secondsToDhm(durationTemp);

                setDistanceState(distanceTemp + " km");
                setDurationState(durationTempString);
                
            }
            else if (status === "ZERO_RESULTS") {   
                messageApi.open({
                    type: "error",
                    content: "Cannot build such a route!",
                });
            }
            else {
                messageApi.open({
                    type: "error",
                    content: "Something went wrong!"
                });
                console.log(status);
            }
        }).catch(() => {

        })
        /* if (results.status === window.google.maps.DirectionsStatus.OK) {
            const temp = results.routes[0].legs.length - 1;
            originRef.current.value = results.routes[0].legs[0].start_address;
            destiantionRef.current.value = results.routes[0].legs[temp].end_address;

            setRouteForSlice(results);
        } */

        /* setDirectionsResponse(results) */

        /* let distanceTemp = 0;
        let durationTemp = 0;

        for (let i = 0; i < results.routes[0].legs.length; i++) {
            distanceTemp += results.routes[0].legs[i].distance.value;
            durationTemp += results.routes[0].legs[i].duration.value;
        }

        distanceTemp = (distanceTemp / 1000).toFixed(1);
        let durationTempString = secondsToDhm(durationTemp);

        setDistanceState(distanceTemp + " km");
        setDurationState(durationTempString); */
    }

    async function calculateRoute() {
        if (originRef.current.value === '' || destiantionRef.current.value === '') {
            return
        }
        // eslint-disable-next-line no-undef
        const directionsService = new google.maps.DirectionsService()
        const results = await directionsService.route({
            origin: originRef.current.value,
            destination: destiantionRef.current.value,
            waypoints: wayPointsRef.current.value ? [wayPointsRef.current.value] : null,
            // eslint-disable-next-line no-undef
            travelMode: google.maps.TravelMode.DRIVING,
        })
        /* [wayPointsRef.current.value] */
        if (results.status === window.google.maps.DirectionsStatus.OK) {
            setRouteForSlice(results);
        }

        setDirectionsResponse(results)
        setDistanceState(results.routes[0].legs[0].distance.text)
        setDurationState(results.routes[0].legs[0].duration.text)
    }

    function deleteLastLeg() {

    }

    function deleteLastRoute() {
        deleteLastLeg();

        dispatch(deletePreviousRoute());
    }

    function clearRoute() {
        setDirectionsResponse(null)
        setDistanceState('')
        setDurationState('')
        originRef.current.value = ''
        destiantionRef.current.value = ''

        setMarkers([]);

        dispatch(cleanRouteData());
    }

    return (
        <Row data-testid="todo-1" className="map_styling">
            <Flex
                flexDirection='column'
                alignItems='center'
                h='100%'
                w='100%'
            >
                {contextHolder}
                <Box position='absolute' left={0} top={0} h='100%' w='100%'>
                    {/* Google Map Box */}
                    <GoogleMap
                        center={center}
                        zoom={8}
                        mapContainerStyle={{ width: '100%', height: '100%' }}
                        options={options}
                        onLoad={map => setMap(map)}
                        onDblClick={onMapClick}
                    >
                        {markers ? markers.map((marker, index) => (
                            <Marker
                                key={index}
                                position={{
                                    lat: parseFloat(marker.lat),
                                    lng: parseFloat(marker.lng)
                                }}

                                onRightClick={() => onMarkerRightClick(marker)}
                            />
                        )) : null}

                        {directionsResponse && (
                            <DirectionsRenderer directions={directionsResponse} />
                        )}
                    </GoogleMap>
                </Box>
                <Row style={{ display: "flex", flexDirection: "row", flexWrap: "nowrap", justifyContent: "center" }}>
                    <Box
                        p={4}
                        borderRadius='lg'
                        m={4}
                        bgColor='white'
                        shadow='base'
                        zIndex='1'
                    >
                        <HStack spacing={2} justifyContent='space-between'>
                            <Box flexGrow={1}>
                                <Autocomplete>
                                    <Input type='text' placeholder='Origin' ref={originRef} />
                                </Autocomplete>
                            </Box>
                            <Box flexGrow={1}>
                                <Autocomplete>
                                    <Input
                                        type='text'
                                        placeholder='Destination'
                                        ref={destiantionRef}
                                    />
                                </Autocomplete>
                            </Box>

                            <ButtonGroup>
                                <Button colorScheme='blue' type='submit' onClick={calculateRoute}>
                                    Calculate Route
                                </Button>
                                <IconButton
                                    aria-label='center back'
                                    icon={<FaTimes />}
                                    onClick={clearRoute}
                                />
                            </ButtonGroup>
                        </HStack>
                        <HStack spacing={4} mt={2} justifyContent='space-between'>

                            <Text>Distance: {distanceState} </Text>
                            <Text>Duration: {durationState} </Text>

                            <IconButton
                                aria-label='center back'
                                icon={<FaBackspace />}
                                isRound
                                onClick={() => {
                                    deleteLastRoute()
                                }}
                            />

                        </HStack>
                    </Box>
                    <Box
                        p={4}
                        borderRadius='lg'
                        m={4}
                        bgColor='white'
                        shadow='base'
                        zIndex='1'
                    >
                        <HStack spacing={2} justifyContent='space-between'>
                            <Box flexGrow={1}>
                                <Autocomplete>
                                    <Input type='text' placeholder='Waypoint' ref={wayPointsRef} />
                                </Autocomplete>
                            </Box>

                            <ButtonGroup>
                                <Button colorScheme='blue' type='submit' onClick={calculateRoute}>
                                    Add
                                </Button>
                            </ButtonGroup>
                        </HStack>

                    </Box>
                </Row>
            </Flex>
        </Row>
    )
}

export default MapContainer;