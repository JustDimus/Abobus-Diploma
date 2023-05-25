import { jwtService } from "../../app/jwtService";
import { selectAuthorization, setAuthorization } from "../../app/slice/authorizationSlice";
import { notification } from "antd";
import { MailOutlined, SettingOutlined } from '@ant-design/icons';
import { Navigate, Outlet, Route, Routes, useNavigate } from "react-router-dom";
import {
    selectNotification,
    setErrorMessage,
    setMessage,
} from "../../app/slice/notificationSlice";
import { useEffect, useState } from "react";
import { Menu } from "antd"
import Login from "../authorization/Login";
import Registration from "../authorization/Registration";
import { useDispatch, useSelector } from "react-redux";

const CustomMenu = () => {
    const [handle, setHandle] = useState('');

    const dispatch = useDispatch();
    const state = useSelector(selectAuthorization);
    const notifications = useSelector(selectNotification);
    const navigate = useNavigate();

    const itemsAuth = [
        {
            label: 'Log In',
            icon: <MailOutlined />,
            key: '/Auth/Login',

        },
        {
            label: 'Sign In',
            icon: <SettingOutlined />,
            key: '/Auth/Registration',
        },
    ];

    const itemLogout = [
        {
            label: 'Log out',
            icon: <MailOutlined />,
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

            {state.isAuthorize || jwtService.get() ? (
                <>
                    <Menu onClick={({ key }) => navigationClick(key)} mode="horizontal"
                        theme="dark" direction="rtl" items={itemLogout}
                        style={{
                            borderRight: 0,
                            backgroundColor: "black",
                        }}
                    >
                    </Menu>
                </>
            ) : (
                <>
                    <Menu onClick={navigationClick} mode="horizontal"
                        theme="dark" direction="rtl" items={itemsAuth}
                        style={{
                            borderRight: 0,
                            backgroundColor: "black",
                        }}
                    >
                    </Menu>
                </>
            )}

        </>
    );
}

function Content() {
    return (
        <>
            <Routes>
                <Route path="/Login" element={<Login />} />
                <Route path="/Register" element={<Registration />} />
            </Routes>
        </>
    );
}

export default CustomMenu;