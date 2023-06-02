import { useDispatch, useSelector } from "react-redux";
import React, { useEffect } from "react";
import { jwtService } from "../../app/jwtService";
import { selectAuthorization, setAuthorization } from "../../app/slice/authorizationSlice";
import {
    selectNotification,
    setErrorMessage,
    setMessage,
} from "../../app/slice/notificationSlice";
import { notification, Row } from "antd";
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
import { FaLocationArrow, FaTimes } from 'react-icons/fa'

const center = { lat: 50.450001, lng: 30.523333 }

const MapContainer = () => {
    const dispatch = useDispatch();
    const state = useSelector(selectAuthorization);
    const notifications = useSelector(selectNotification);
    const navigate = useNavigate();

    const { isLoaded } = useJsApiLoader({
        googleMapsApiKey: process.env.REACT_APP_GOOGLE_MAPS_API_KEY,
        libraries: ['places'],
    });

    const [map, setMap] = useState(/** @type google.maps.Map */(null))
    const [directionsResponse, setDirectionsResponse] = useState(null)
    const [distance, setDistance] = useState('')
    const [duration, setDuration] = useState('')

    const originRef = useRef()
    /** @type React.MutableRefObject<HTMLInputElement> */
    const destiantionRef = useRef()

    const openErrorNotification = () => {
        notification["error"]({
            message: "Error",
            description: notifications.errorMessage,
        });
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
    }, [dispatch]);

    if (!isLoaded) {
        return <SkeletonText />
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
            // eslint-disable-next-line no-undef
            travelMode: google.maps.TravelMode.DRIVING,
        })
        setDirectionsResponse(results)
        setDistance(results.routes[0].legs[0].distance.text)
        setDuration(results.routes[0].legs[0].duration.text)
    }

    function clearRoute() {
        setDirectionsResponse(null)
        setDistance('')
        setDuration('')
        originRef.current.value = ''
        destiantionRef.current.value = ''
    }

    return (
        <Row className="map_styling">
            <Flex
                flexDirection='column'
                alignItems='center'
                h='100%'
                w='100%'
            >
                <Box position='absolute' left={0} top={0} h='100%' w='100%'>
                    {/* Google Map Box */}
                    <GoogleMap
                        center={center}
                        zoom={15}
                        mapContainerStyle={{ width: '100%', height: '100%'}}
                        options={{
                            zoomControl: false,
                            streetViewControl: false,
                            mapTypeControl: false,
                            fullscreenControl: false,
                        }}
                        onLoad={map => setMap(map)}
                    >
                        <Marker position={center} />
                        {directionsResponse && (
                            <DirectionsRenderer directions={directionsResponse} />
                        )}
                    </GoogleMap>
                </Box>
                <Box
                    p={4}
                    borderRadius='lg'
                    m={4}
                    bgColor='white'
                    shadow='base'
                    minW='container.md'
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
                            <Button colorScheme='pink' type='submit' onClick={calculateRoute}>
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

                        <Text>Distance: {distance} </Text>
                        <Text>Duration: {duration} </Text>

                        <IconButton
                            aria-label='center back'
                            icon={<FaLocationArrow />}
                            isRound
                            onClick={() => {
                                map.panTo(center)
                                map.setZoom(15)
                            }}
                        />

                    </HStack>
                </Box>
            </Flex>
        </Row>
    )
}

export default MapContainer;