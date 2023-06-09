import React from 'react';
import { useEffect, useState } from "react";
import { Form, Input, Button, Checkbox, Typography, Row, Divider } from "antd";
import { GoogleLogin } from "react-google-login";
import { selectAuthorization } from "../../app/slice/authorizationSlice";
import {
    loginAsync,
    loginThunk,
    loginViaGoogleAsync,
    selectLogin,
    setLoginUsername,
    setLoginPassword,

} from "../../app/slice/authorization/loginSlice";
import { useNavigate } from "react-router-dom";
import { gapi } from "gapi-script";
import { useDispatch, useSelector } from 'react-redux';
import { UserOutlined, LockOutlined, GoogleOutlined, ContactsOutlined } from "@ant-design/icons"
/* import Typography from 'antd/es/typography/Typography'; */

const Login = () => {
    const clientId = "878304166832-pjiqjljvokfdvrsev0835p01i6e9m6nm.apps.googleusercontent.com";
    const dispatch = useDispatch();
    const state = useSelector(selectLogin);
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
            <Row type="flex" justify="center" align="middle" style={{ paddingTop: "40px", paddingBottom: "20px" }}>
                <Form
                    className="authorization_form"
                    name="login"
                    initialValues={{ remember: true }}
                    onFinish={logInSystem}
                    onFinishFailed={(error) => {
                        console.log({ error });
                    }}
                >

                    <div className="headlines_main">Login</div>

                    <Form.Item
                        /* label="Login " */
                        name="login"
                        rules={[{
                            required: true,
                            message: "Login is too short!",
                            min: 2,
                            whitespace: false
                        },
                        {
                            required: true,
                            message: "Login is too long!",
                            max: 20,
                            whitespace: false
                        },
                        {
                            required: false,
                            message: "Please use latin characters!",
                            max: 20,
                            whitespace: false,
                            pattern: new RegExp(/^[A-Za-z0-9]+$/i),
                        },]}
                        hasFeedback
                    >
                        <Input

                            prefix={<UserOutlined style={{ color: 'rgba(0,0,0,.25)', paddingRight: "5px" }} />}
                            placeholder="Enter your login"
                            value={state.email}
                            onChange={(e) => setEmail(e)}
                        />
                    </Form.Item>

                    <Form.Item
                        name="password"
                        rules={[
                            {
                                required: true, message: "Enter your password!"
                            },
                            {
                                min: 4,
                                message: "Password is too short!"
                            }
                        ]}
                    >
                        <Input.Password
                            prefix={<LockOutlined style={{ color: 'rgba(0,0,0,.25)', paddingRight: "5px" }} />}
                            placeholder="Enter valid password"
                            value={state.password}
                            onChange={(e) => setPassword(e)}
                        />
                    </Form.Item>

                    <Form.Item
                        name="remember"
                        valuePropName="checked"
                    >
                        <div className="remember_me_and_forgot_password">
                            <Checkbox>Remember me</Checkbox>
                            <a className="forgot_password_styling" href="#" >
                                Forgot password?
                            </a>
                        </div>

                    </Form.Item>

                    <Form.Item >
                        <Button type="primary" htmlType="submit" className="main_btns"
                            onClick={(res) => {
                                console.log("Aboba")
                            }}
                            onError={(error) => {
                                console.log(error);
                            }}
                            block
                        >
                            Log In
                        </Button>
                    </Form.Item>

                    <Divider style={{ borderColor: "black" }}><div style={{ paddingBottom: "4px" }}>or</div></Divider>

                    <div className="socials">
                        <Form.Item>
                            <GoogleLogin clientId={clientId}
                                buttonText="Log In via Google"
                                isSignedIn={true}
                                onSuccess={(res) => {
                                    console.log("Google Login")
                                    loginGoogleAsync(res);
                                }}

                                onFailure={(error) => {
                                    console.log(error)
                                }}
                            />
                        </Form.Item>
                    </div>

                </Form>
            </Row>

        </>
    );
}

export default Login;