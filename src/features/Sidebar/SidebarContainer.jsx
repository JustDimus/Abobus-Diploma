import { Menu } from "antd";
import Sider from "antd/lib/layout/Sider";
import { selectAuthorization } from "../../app/slice/authorizationSlice";
import { jwtService } from "../../app/jwtService";
import { useNavigate } from "react-router-dom";
import { UserOutlined, EditOutlined, AimOutlined, BookOutlined, HistoryOutlined, HomeOutlined } from '@ant-design/icons';
import { useSelector } from "react-redux";
import MapContainer from "../main/MapContainer";
import AccountContainer from "../common/account/AccountContainer";

const SidebarContainer = () => {
    const state = useSelector(selectAuthorization);
    const navigation = useNavigate();

    const items = [
        {
            key: '',
            icon: <HomeOutlined />,
            label: 'Home',
        },
        {
            key: 'Account/User',
            icon: <UserOutlined />,
            label: 'Account',
            content: <AccountContainer />
        },
        {
            key: 'Account/Map',
            icon: <AimOutlined />,
            label: 'Map',
            content: <MapContainer />,
        },
        {
            key: 'Account/History',
            icon: <HistoryOutlined />,
            label: 'History',
        },
        {
            key: 'About',
            icon: <BookOutlined />,
            label: 'About',
        },
        {
            key: 'Faq',
            icon: <EditOutlined />,
            label: 'FAQ',
        },
    ];

    const handleClick = (e) => {
        navigation("/" + e.key);
    }

    return (
        <>
            {jwtService.get() || state.isAuthorize ? (
                (
                    <Sider width={200}
                        style={{
                            overflow: 'auto',
                            height: '100%',
                            minHeight: '100vh',
                            position: 'sticky',
                            left: 0,
                            top: 0,
                            bottom: 0,

                        }}
                    >
                        <Menu

                            items={items}
                            mode="inline"
                            defaultSelectedKeys={["1"]}
                            defaultOpenKeys={["sub1"]}
                            style={{
                                height: "100%",
                                borderRight: 0,
                                backgroundColor: "green",
                                paddingTop: "10px"
                            }}
                            theme="dark"
                            onClick={handleClick}
                        >

                        </Menu>
                    </Sider>
                )
            ) : (
                <Sider width={200} className="site-layout-background"
                    style={{
                        overflow: 'auto',
                        height: '100%',
                        minHeight: '100vh',
                        position: 'sticky',
                        left: 0,
                        top: 0,
                        bottom: 0,
                        backgroundColor: "white",
                    }}

                >
                    <Menu

                        items={items}
                        mode="inline"
                        defaultSelectedKeys={["1"]}
                        defaultOpenKeys={["sub1"]}
                        style={{
                            height: "100%",
                            borderRight: 0,
                            /* backgroundColor: "rgb(248,248,255)", */
                            paddingTop: "10px"
                        }}
                        theme="light"
                        onClick={handleClick}
                    >

                    </Menu>
                </Sider>
            )}
        </>
    )
};

export default SidebarContainer;