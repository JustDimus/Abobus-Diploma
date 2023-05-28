import { jwtService } from "../../app/jwtService";
import { selectAuthorization, setAuthorization } from "../../app/slice/authorizationSlice";
import { notification } from "antd";
import { MailOutlined, SettingOutlined, LoginOutlined, LogoutOutlined, UsergroupAddOutlined } from '@ant-design/icons';
import { Link, Navigate, Outlet, Route, Routes, useNavigate } from "react-router-dom";
import {
    selectNotification,
    setErrorMessage,
    setMessage,
} from "../../app/slice/notificationSlice";
import React, { useEffect, useState } from "react";
import { Menu } from "antd"
import Login from "../authorization/Login";
import Registration from "../authorization/Registration";
import { useDispatch, useSelector } from "react-redux";
import { HomeContainer } from "../home/index"

const CustomMenu = () => {
    const [handle, setHandle] = useState('');

    const dispatch = useDispatch();
    const state = useSelector(selectAuthorization);
    const notifications = useSelector(selectNotification);
    const navigate = useNavigate();

    const itemsNotAuth = [
        {
            label: 'Log In',
            icon: <LoginOutlined />,
            key: '/Auth/Login',
        },
        {
            label: 'Sign In',
            icon: <UsergroupAddOutlined />,
            key: '/Auth/Registration',
        },
    ];

    const itemsAuth = [
        {
            label: 'Log out',
            icon: <LogoutOutlined />,
            key: '/Logout',

        },
    ];

    const openErrorNotification = () => {
        notification["error"]({
            message: "Error",
            description: notifications.errorMessage,
        });
        dispatch(setErrorMessage(undefined));
    };

    const openNotification = () => {
        notification["success"]({
            message: "Note",
            description: notifications.message,
        });
        dispatch(setMessage(undefined));
    }

    const navigationClick = (e) => {
        if (e === "/Logout") {
            jwtService.remove();
            navigate("/");
        } else {
            navigate(e.key);
        }
    }

    useEffect(() => {
        if (jwtService.get()) {
            dispatch(setAuthorization(true));
        }
    }, [dispatch]);

    useEffect(() => {
        <Navigate to={handle} />
    }, [navigationClick])

    return (
        <>
            {notifications ? (notifications.errorMessage ? openErrorNotification() : null) : null}
            {notifications ? (notifications.message ? openNotification() : null) : null}

            <div className="header_menu">
                <Link to={"/"} className="logo_main">Abobus</Link>

                {state.isAuthorize || jwtService.get() ? (
                    <>
                        <Menu onClick={({ key }) => navigationClick(key)} mode="horizontal" className="header_menu"
                            theme="dark" items={itemsAuth} style={{ backgroundColor: "black", flex: "auto", placeContent: "flex-end" }}
                        >
                        </Menu>
                    </>
                ) : (
                    <>
                        <Menu onClick={navigationClick} mode="horizontal"
                            items={itemsNotAuth} theme="dark" style={{ backgroundColor: "black", flex: "auto", placeContent: "flex-end" }}
                        >
                        </Menu>
                    </>
                )}
            </div>
        </>
    );
}

export default CustomMenu;