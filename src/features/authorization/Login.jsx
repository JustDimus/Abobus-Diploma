import React from 'react';
import { useEffect, useState } from "react";
import { Form, Input, Button, Checkbox } from "antd";
import { GoogleLogin, GoogleLoginResponse } from "react-google-login";
import {
    loginAsync,
    LoginState,
    loginThunk,
    loginViaGoogleAsync,
    selectLogin,
    setLoginUsername,
    setLoginPassword,

} from "../../app/slice/authorization/loginSlice";
import { useNavigate } from "react-router-dom";
import { gapi } from "gapi-script";
import { useDispatch, useSelector } from 'react-redux';

const Login = () => {
    const clientId = "1098295275668-msaisicftbh0is9pusd5bgchpb4tq57g.apps.googleusercontent.com";
    const dispatch = useDispatch();
    const state = useState(selectLogin);
    const authorization = useSelector(selectAuthorization);
    const navigate = useNavigate();

    useEffect(() => {
        const initClient = () => {
            gapi.auth2.init({
                clientId: clientId,
                scope: ''
            });
        };
        gapi.load('client:auth2', initClient);
    });

    const logInSystem = () => {
        console.log("logged in")
        dispatch(loginThunk)
        debugger;
        navigateUser();
    }

    const navigateUser = () => {
        console.log("navigated!")
    }

    const setEmail = (e) => {
        dispatch(setLoginUsername(e.target.value));
    }

    const setPassword = (e) => {
        dispatch(setLoginPassword(e.target.value));
    }

    const loginGoogleAsync = async (res) => {
        await dispatch(loginViaGoogleAsync(res));
        navigateUser();
    }

    return (
        <>
            <h3 style={{ textAlign: "center" }}>Login</h3>
            <Form
                style={{ paddingTop: "1em" }}
                name="login"
                labelCol={{ span: 8 }}
                wrapperCol={{ span: 8 }}
                initialValues={{ remember: true }}
                onFinish={logInSystem}
                onFinishFailed={(error) => {
                    console.log({ error });
                }}
            >
                <Form.Item
                    label="Login "
                    name="login"
                    rules={[{ required: true, message: "Please input your login" }]}
                >
                    <Input
                        value={state.email}
                        onChange={(e) => setEmail(e)}
                    />
                </Form.Item>

                <Form.Item
                    label="Password"
                    name="password"
                    rules={[{ required: true, message: "Please input your password." }]}
                >
                    <Input.Password
                        value={state.password}
                        onChange={(e) => setPassword(e)}
                    />
                </Form.Item>

                <Form.Item
                    name="remember"
                    valuePropName="checked"
                    wrapperCol={{ offset: 8, span: 16 }}
                >
                    <Checkbox>Remember me</Checkbox>
                </Form.Item>

                <Form.Item wrapperCol={{ offset: 8, span: 16 }}>
                    <Button type="primary" htmlType="submit"
                        onClick={(res) => {
                            console.log("Aboba")
                        }}
                        onError={(error) => {
                            console.log(error);
                        }}
                    >
                        Sign In
                    </Button>
                </Form.Item>

                <Form.Item wrapperCol={{ offset: 8, span: 16 }}>
                    <GoogleLogin clientId={clientId}
                        buttonText="Sign In via Google"
                        isSignedIn={true}
                        onSuccess={(res) => {
                            console.log("Hello")
                            loginGoogleAsync(res);
                        }}

                        onFailure={(error) => {
                            console.log(error)
                        }}
                    />
                </Form.Item>
            </Form>
        </>
    );
}

export default Login;