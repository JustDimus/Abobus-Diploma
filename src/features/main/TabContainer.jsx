import { useDispatch, useSelector } from "react-redux";
import React, { useEffect } from "react";
import { jwtService } from "../../app/jwtService";
import { selectAuthorization, setAuthorization } from "../../app/slice/authorizationSlice";
import {
    selectNotification,
    setErrorMessage,
    setMessage,
} from "../../app/slice/notificationSlice";
import { notification } from "antd";
import { useNavigate } from "react-router-dom";
import { Tabs, TabPane } from "antd";
import { AimOutlined, BookOutlined, HistoryOutlined } from "@ant-design/icons";
import NotAuthorizedContainer from "../common/NotAuthorizedContainer";
import {
    useJsApiLoader,
    GoogleMap,
    Marker,
    Autocomplete,
    DirectionsRenderer,
} from '@react-google-maps/api'
import { useRef, useState } from 'react'
import MapContainer from "./MapContainer";

const TabContainer = () => {

    const dispatch = useDispatch();
    const state = useSelector(selectAuthorization);
    const notifications = useSelector(selectNotification);
    const navigate = useNavigate();

    const items = [
        {
            label: 'Account',
            icon: <BookOutlined />,
            content: '/Account/User',
            /* key: '/Account/User' */
        },
        {
            label: 'Map',
            icon: <AimOutlined />,
            content:  "da",
            /* key: '/Account/Map' */
        },
        {
            label: 'History',
            icon: <HistoryOutlined />,
            content: '/Account/History',
            /* key: '/Account/History' */
        },
    ];
    {/* <MapContainer /> */ }
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

    /* return (
        <>
            {notifications ? (notifications.errorMessage ? openErrorNotification() : null) : null}
            {notifications ? (notifications.message ? openNotification() : null) : null}

            {state.isAuthorize || jwtService.get() ? (
                <>
                    <Tabs
                        defaultActiveKey="1"
                        tabPosition="left"
                        style={{
                            height: "100vh",
                        }}
                        items={items.map((item, i) => {
                            const id = String(i + 1);
                            return {
                                label: (
                                    <>
                                        <span>
                                            {item.icon}
                                        </span>
                                        <span>
                                            {item.label}
                                        </span>
                                    </>

                                ),
                                key: id,
                                children: "children of tab",
                            };
                        })}
                        onClick={({ e }) => navigationClick(e)}
                    />
                </>
            ) : (
                <>
                    <div className="tab_main">
                        <Tabs
                            defaultActiveKey="1"
                            tabPosition="left"
                            style={{
                                height: "100vh",
                            }}
                            items={items.map((item, i) => {
                                const id = String(i + 1);
                                return {
                                    label: (
                                        <>
                                            <span>
                                                {item.icon}
                                            </span>
                                            <span>
                                                {item.label}
                                            </span>
                                        </>

                                    ),
                                    key: id,
                                    children: item.content,
                                };
                            })}
                        />
                    </div>

                </>

            )}

        </>
    ) */
}
{/* <NotAuthorizedContainer /> */ }
export default TabContainer;