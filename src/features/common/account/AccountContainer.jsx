import { Button, Form, Input, Upload, Image, Row, Col, Card, Meta } from "antd";
import { useEffect, useState } from "react";
import { UploadOutlined, HomeOutlined, PushpinOutlined } from "@ant-design/icons";
import {
    loadUserAsync,
    selectUser,
    setAddress,
    setFirstName,
    setLastName,
    setPhone,
    setUserCity,
    updateUserThunk,
} from "../../../app/slice/user/userSlice";
import { useDispatch, useSelector } from "react-redux";
/* import main_image4 from '../../../../public/images' */

const AccountContainer = () => {
    const dispatch = useDispatch();
    const state = useSelector(selectUser);
    const [file, chooseFile] = useState(new File([], ""));
    const [isFileRemoved, removeFile] = useState(true);
    useEffect(() => {
        if (!state.name) {
            dispatch(loadUserAsync());
        }
    }, [dispatch, state.name]);
    return (
        <>
            <Row className="account_main_container" style={{ paddingTop: "40px" }}>
                <Col span={16}>
                    <div className="account_tochange_row">
                        <Form
                            layout={"vertical"}
                            onFinish={() => {
                                if (!isFileRemoved) dispatch(updateUserThunk(file));
                                else dispatch(updateUserThunk(null));
                            }}
                        >
                            <div style={{ fontFamily: "Lobster", fontSize: "20px", paddingBottom: "15px" }}>
                                Do you want to change it?
                            </div>

                            <Form.Item name="firstName">
                                <Input
                                    value={state.name}
                                    prefix={<HomeOutlined style={{ color: 'rgba(0,0,0,.25)', paddingRight: "5px" }} />}
                                    placeholder="Here you can change your name"
                                    onChange={(e) => dispatch(setFirstName(e.target.value))}
                                />
                            </Form.Item>
                            <Form.Item name="lastName">
                                <Input
                                    value={state.surname}
                                    prefix={<HomeOutlined style={{ color: 'rgba(0,0,0,.25)', paddingRight: "5px" }} />}
                                    placeholder="Here you can change your surname"
                                    onChange={(e) => dispatch(setLastName(e.target.value))}
                                />
                            </Form.Item>
                            <Form.Item>
                                <Button htmlType="submit" type="primary">
                                    Change
                                </Button>
                            </Form.Item>

                        </Form>
                    </div>
                </Col>

                <Col span={8}>
                    <Row className="general_info_block">
                        <Form>
                            {/* <Card cover={<div style={{ height: "200px", backgroundSize: "cover", backgroundImage: "https://os.alipayobjects.com/rmsportal/QBnOOoLaAfKPirc.png" }} />}>

                            </Card> */}
                            <Card /* bodyStyle={{ padding: "0" }} */ cover={<img src={require('../../../images/main_image2.jpg')} className="background_image_user" />}>

                                {/* <div style={{ paddingLeft: "20px", fontFamily: "Lobster", fontSize: "20px" }}>
                                    Infromation
                                </div> */}

                                <Row style={{ display: "flex", justifyContent: "center", flexDirection:"column", alignContent:"space-around"}}>
                                    <Col>
                                        <img className="user_avatar" src={require('../../../images/main_image.jpg')} />

                                    </Col>
                                    <Col style={{textAlign:"center"}} className="additional_info">
                                        <h1 style={{fontSize: "16px", fontWeight:"bold"}}>Bohdan Butenko</h1>
                                        <p>noobbogdan@gmail.com</p>
                                        <p>Odesa, Ukraine</p>
                                    </Col>  
                                </Row>
                            </Card>

                        </Form>
                    </Row>
                    <div className="select_image_block">
                        <Form>
                            <div style={{ paddingLeft: "20px", fontFamily: "Lobster", fontSize: "20px" }}>
                                Select Profile Photo
                            </div>
                            <div style={{ display: "flex", paddingLeft: "20px", paddingTop: "15px", paddingRight: "30px" }}>
                                <div className="image_and_pin">
                                    <Form.Item name="file">
                                        <Image
                                            width={100}
                                            src={state.image}
                                            style={{ borderRadius: "14px" }}
                                            fallback="data:image/png;base64,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"
                                        />


                                    </Form.Item>
                                    <Form.Item style={{ paddingLeft: "20px" }}>
                                        <PushpinOutlined style={{ fontSize: "40px" }} />
                                    </Form.Item>
                                </div>

                                <div style={{ paddingLeft: "20px", display: "flex", flexDirection: "column", alignItems: "stretch", }}>
                                    <Form.Item >
                                        <Upload
                                            onRemove={(e) => removeFile(true)}
                                            action="https://www.mocky.io/v2/5cc8019d300000980a055e76"
                                            listType="picture"
                                            onChange={(e) => {
                                                removeFile(false);
                                                chooseFile(e.file.originFileObj);
                                            }}
                                            maxCount={1}
                                            block
                                        >
                                            <Button type="primary" icon={<UploadOutlined />} block>Upload File</Button>
                                        </Upload>

                                    </Form.Item>
                                    <span>JPG, PNG. Max size of 800K</span>
                                </div>
                            </div>
                        </Form>

                    </div>
                </Col>

            </Row>
        </>
    );
};
export default AccountContainer;