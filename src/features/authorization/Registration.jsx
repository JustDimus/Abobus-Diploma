import { useState } from "react";
import { selectAuthorization } from "../../app/slice/authorizationSlice";
import { Form, Input, Button, Row, Col } from "antd";
import {
    setRegistrationName,
    setRegistrationSurname,
    setRegistrationNickname,
    setRegistrationEmail,
    setRegistrationPassword,
    setRegistrationConfirmPassword,
    selectRegistration,
    registrateThunk,
} from "../../app/slice/authorization/registrationSlice";
import Password from "antd/lib/input/Password";
import { useDispatch, useSelector } from "react-redux";
import { LockOutlined, MailOutlined, UserOutlined, HomeOutlined } from "@ant-design/icons"

const Registration = () => {
    const dispatch = useDispatch();
    const state = useSelector(selectRegistration);
    const authorize = useSelector(selectAuthorization).isAuthorize;

    const formItemLayout = {
        labelCol: {
            span: 7
        },
        wrapperCol: {
            span: 10
        }
    };

    const setName = (e) => {
        dispatch(setRegistrationName(e.target.value));
    }

    const setSurname = (e) => {
        dispatch(setRegistrationSurname(e.target.value));
    }

    const setNickname = (e) => {
        dispatch(setRegistrationNickname(e.target.value));
    }

    const setRegEmail = (e) => {
        dispatch(setRegistrationEmail(e.target.value));
    }

    const setRegPassword = (e) => {
        dispatch(setRegistrationPassword(e.target.value));
    }

    const setRegConfirmPassword = (e) => {
        dispatch(setRegistrationConfirmPassword(e.target.value));
    }

    const RegistrateUser = () => {
        dispatch(registrateThunk());
    }

    return (
        <>
            <Row type="flex" justify="center" align="middle" style={{ paddingTop: "20px", paddingBottom: "20px" }}>
                <Form
                    className="authorization_form"
                    hideRequiredMark
                    onFinish={RegistrateUser}
                    style={{ width: "500px" }}
                >

                    <div className="headlines_main">Registration</div>

                    <div className="auth_two_items_in_one_row">
                        <Form.Item
                            name="name"
                            rules={[{
                                required: true,
                                message: "Enter valid name!",
                                min: 2,
                                max: 15,
                                whitespace: false,
                                pattern: new RegExp(/^[a-zA-Z]+$/i),
                            }]}
                            hasFeedback
                        >
                            <Input className="ant_input_suffix"
                                prefix={<HomeOutlined style={{ color: 'rgba(0,0,0,.25)', paddingRight: "5px" }} />}
                                value={state.name}
                                placeholder="Enter your name"
                                onChange={(e) => setName(e)}
                            />

                        </Form.Item>

                        <Form.Item
                            name="surname"
                            rules={[{
                                required: true,
                                message: "Enter valid surname!",
                                min: 2,
                                max: 15,
                                whitespace: false,
                                pattern: new RegExp(/^[a-zA-Z]+$/i),
                            }]}
                            hasFeedback
                        >
                            <Input
                                prefix={<HomeOutlined style={{ color: 'rgba(0,0,0,.25)', paddingRight: "5px" }} />}
                                value={state.surname}
                                placeholder="Enter your surname "
                                onChange={(e) => setSurname(e)}
                            />
                        </Form.Item>
                    </div>

                    <Form.Item
                        name="nickname"
                        rules={[{
                            required: true,
                            message: "Enter valid nickname!",
                            min: 4,
                            max: 15,
                            whitespace: false
                        }]}
                        hasFeedback
                    >
                        <Input
                            prefix={<UserOutlined style={{ color: 'rgba(0,0,0,.25)', paddingRight: "5px" }} />}
                            value={state.nickname}
                            placeholder="Enter your nickname"
                            onChange={(e) => setNickname(e)}
                        />
                    </Form.Item>
                    <Form.Item
                        name="email"
                        rules={[{
                            required: true,
                            message: "Enter valid email!",
                            type: "email",
                            min: 6,
                            max: 30,
                            whitespace: false
                        }]}
                        hasFeedback
                    >
                        <Input
                            prefix={<MailOutlined style={{ color: 'rgba(0,0,0,.25)', paddingRight: "5px" }} />}
                            value={state.email}
                            placeholder="Enter your email"
                            onChange={(e) => setRegEmail(e)}
                        />
                    </Form.Item>

                    <Form.Item
                        name="Password"
                        rules={[{
                            required: true,
                            message: "Enter valid password!!",
                            min: 5,
                            max: 30,
                            whitespace: true
                        }]}
                        hasFeedback
                    >
                        <Password
                            prefix={<LockOutlined style={{ color: 'rgba(0,0,0,.25)', paddingRight: "5px" }} />}
                            value={state.password}
                            placeholder="Enter your password"
                            onChange={(e) => setRegPassword(e)}
                        />
                    </Form.Item>
                    <Form.Item
                        name="ConfirmPassword"
                        rules={[
                            {
                                required: true,
                                message: "Incorrect confirmation!!",
                                whitespace: true,
                                min: 5,
                                max: 30,
                            },
                            ({ getFieldValue }) => ({
                                validator(_, value) {
                                    if (!value || getFieldValue("Password") === value) {
                                        return Promise.resolve();
                                    }

                                    return Promise.reject(
                                        new Error("The two passwords doesn't match")
                                    )
                                },
                            }),
                        ]}

                        dependencies={["Password"]}
                        hasFeedback
                    >
                        <Password
                            prefix={<LockOutlined style={{ color: 'rgba(0,0,0,.25)', paddingRight: "5px" }} />}
                            value={state.confirmPassword}
                            placeholder="Confirm your password"
                            onChange={(e) => setRegConfirmPassword(e)}
                        />
                    </Form.Item>

                    <Form.Item
                    >
                        <Button className="main_btns"
                            type="primary"
                            htmlType="submit"
                            block
                        >
                            Registrate
                        </Button>
                    </Form.Item>
                </Form>
            </Row>


        </>
    );
};

export default Registration;