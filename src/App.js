import Layout, { Content, Footer, Header } from "antd/lib/layout/layout";
import { BrowserRouter as Router, Outlet, Route, Routes } from "react-router-dom";
import './App.css';
import Registration from "./features/authorization/Registration";
import Login from "./features/authorization/Login";
import HomeContainer from "./features/home/HomeContainer";
import FooterContainer from "./features/common/footer/FooterContainer";
import HeaderContainer from "./features/common/header/HeaderContainer";
import CustomMenu from "./features/menu/CustomMenu";
import BackgroundImage from "./features/common/background/BackgroundImage";
import NotFoundContainer from "./features/common/NotFoundContainer";
import TabContainer from "./features/main/TabContainer";
import MapContainer from "./features/main/MapContainer";
import SidebarContainer from "./features/Sidebar/SidebarContainer";
import AccountContainer from "./features/common/account/AccountContainer";
/* import Account from "./features/Account"; */

function App() {
  return (
    <>
      <Router>
        <Layout className="layout">
          <Header className="header">
            <CustomMenu />
          </Header>

          <Layout style={{ minHeight: "100vh" }}>
            <SidebarContainer />
            <Content className="main_styling">
              <Routes>
                <Route path="/" element={<HomeContainer />} />
                <Route path="Auth/Login" element={<Login />} />
                <Route path="Auth/Registration" element={<Registration />} />
                <Route path="Account/Map" element={<MapContainer />} />
                <Route path="Account/User" element={<AccountContainer />} />
                <Route path="Account/History" element={<TabContainer />} />

                <Route
                  path="*"
                  element={<NotFoundContainer />}
                />

              </Routes>
            </Content>
          </Layout>
          <Footer style={{
            textAlign: "center",
            backgroundColor: "black"
          }}>
            <FooterContainer />
          </Footer>
        </Layout>
      </Router>
    </>
  );
}

export default App;