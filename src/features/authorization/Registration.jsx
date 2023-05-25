import { useState } from "react";
import { selectAuthorization } from "../../app/slice/authorizationSlice";
import { Form, Input, Button, Row, Col } from "antd";
import {
    setRegistrationName,
    setRegistrationSurname,
    setRegistrationNickname,
    setRegistrationGender,
    setRegistrationEmail,
    setRegistrationPassword,
    setRegistrationConfirmPassword,
    selectRegistration,
    registrateThunk,
} from "../../app/slice/authorization/registrationSlice";
import Password from "antd/lib/input/Password";
import { useDispatch, useSelector } from "react-redux";

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

    const setGender = (e) => {
        dispatch(setRegistrationGender(e.target.value));
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
            <h3 style={{ textAlign: "center" }}>Registration</h3>
            <Form
                style={{ paddingTop: "1em" }}
                {...formItemLayout}
                //wrapperCol={{ span: 19 }}
                //labelCol={{ span: 5 }}
                hideRequiredMark
                onFinish={RegistrateUser}
            >
                <Form.Item
                    name="name"
                    label="Name"
                    rules={[{
                        required: true,
                        message: "Enter valid name!",
                        min: 2,
                        whitespace: true
                    }]}
                    hasFeedback
                >
                    <Input
                        value={state.name}
                        placeholder="Enter your name here"
                        onChange={(e) => setName(e)}
                    />

                </Form.Item>

                <Form.Item
                    name="surname"
                    label="Surname"
                    rules={[{
                        required: true,
                        message: "Enter valid surname!",
                        min: 2,
                        whitespace: true
                    }]}
                    hasFeedback
                >
                    <Input
                        value={state.surname}
                        placeholder="Enter your surname here"
                        onChange={(e) => setSurname(e)}
                    />
                </Form.Item>

                <Form.Item
                    name="nickname"
                    label="Nickname"
                    rules={[{
                        required: true,
                        message: "Enter valid nickname!",
                        min: 4,
                        max: 15,
                        whitespace: true
                    }]}
                    hasFeedback
                >
                    <Input
                        value={state.nickname}
                        placeholder="Enter your nickname here"
                        onChange={(e) => setNickname(e)}
                    />
                </Form.Item>
                <Form.Item
                    name="gender"
                    label="Gender"
                    rules={[{
                        required: true,
                        message: "Enter valid gender!",
                        min: 1,
                        max: 1,
                        whitespace: true
                    }]}
                    hasFeedback
                >
                    <Input
                        value={state.gender}
                        placeholder="Choose your gender here"
                        onChange={(e) => setGender(e)}
                    />
                </Form.Item>
                <Form.Item
                    name="email"
                    label="Email"
                    rules={[{
                        required: true,
                        message: "Enter valid email!",
                        type: "email",
                        min: 6,
                        whitespace: true
                    }]}
                    hasFeedback
                >
                    <Input
                        value={state.email}
                        placeholder="Enter your email here"
                        onChange={(e) => setRegEmail(e)}
                    />
                </Form.Item>
                <Form.Item
                    name="Password"
                    label="Password"
                    rules={[{
                        required: true,
                        message: "Enter valid password!!",
                        min: 5,
                        whitespace: true
                    }]}
                    hasFeedback
                >
                    <Password
                        value={state.password}
                        placeholder="Enter your password here"
                        onChange={(e) => setRegPassword(e)}
                    />
                </Form.Item>
                <Form.Item
                    name="ConfirmPassword"
                    rules={[
                        {
                            required: true,
                            message: "Incorrect confirmation!!",
                            whitespace: true
                        },
                        ({ getFieldValue }) => ({
                            validator(_, value) {
                                if (!value || getFieldValue("Password") === value) {
                                    return Promise.resolve();
                                }

                                return Promise.reject(
                                    new Error("The two passwords that you entered doesn't match")
                                )
                            },
                        }),
                    ]}

                    label="Confirm Password"
                    dependencies={["Password"]}
                    hasFeedback
                >
                    <Password
                        value={state.confirmPassword}
                        placeholder="Confirm your password"
                        onChange={(e) => setRegConfirmPassword(e)}
                    />
                </Form.Item>

                <Form.Item
                    wrapperCol={{ span: 8, offset: 8 }}
                >
                    <Button
                        block
                        type="primary"
                        htmlType="submit"
                    >
                        Registrate
                    </Button>
                </Form.Item>
            </Form>

        </>
    );
};

export default Registration;