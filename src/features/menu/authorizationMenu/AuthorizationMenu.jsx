import { Menu } from "antd";
import { SetStateAction, useState, useEffect } from "react";
import { Link, BrowserRouter as Router, Route, Routes, useNavigate, Navigate } from "react-router-dom";
import Login from "../../authorization/Login";
import { AppstoreOutlined, MailOutlined, SettingOutlined } from '@ant-design/icons';
import Registration from "../../authorization/Registration";
import { jwtService } from "../../../app/jwtService";
import { selectAuthorization, setAuthorization } from "../../../app/slice/authorizationSlice";
import { useAppDispatch, useAppSelector } from "../../../app/hooks";
import { useDispatch, useSelector } from "react-redux";


const items = [
    {
        label: 'Log In',
        icon: <MailOutlined />,
        key: 'Login',

    },
    {
        label: 'Sign In',
        icon: <SettingOutlined />,
        key: 'Registration',
    },
];

const items2 = [
    {
        label: 'Log In',
        icon: <MailOutlined />,
        key: 'Login',
    },
    {
        label: 'Sign In',
        icon: <SettingOutlined />,
        key: 'Registration',
    },
    {
        label: 'Home',
        icon: <SettingOutlined />,
        key: 'Home',
    }
];

const AuthorizationMenu = () => {
    const dispatch = useDispatch();
    const [current, setCurrent] = useState("Login");
    const state = useSelector(selectAuthorization);
    const navigate = useNavigate();
    const onClick = () => {
        setCurrent(e.key);
        <Navigate to={current}/>
        //console.log("aboba");
        //navigate("/Login", { replace: true })
    };

    // return (
    //     <>
    //         {jwtService.get() || state.isAuthorize ? (
    //             <>
    //                 <Menu onClick={onClick} mode="horizontal" theme="dark" selectedKeys={[current]} items={items2} />;
    //             </>
    //         ) : (
    //             <>
    //                 <Menu onClick={onClick} mode="horizontal" theme="dark" selectedKeys={[current]} items={items} key="2" />;
    //             </>
    //         )}
    //     </>
    // );
}

export default AuthorizationMenu;