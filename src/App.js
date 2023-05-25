import Layout, { Content, Footer, Header } from "antd/lib/layout/layout";
import { BrowserRouter as Router, Outlet, Route, Routes } from "react-router-dom";
import './App.css';
import Registration from "./features/authorization/Registration";
import Login from "./features/authorization/Login";
import HomeContainer from "./features/home/HomeContainer";
import FooterContainer from "./features/common/footer/FooterContainer";
import HeaderContainer from "./features/common/header/HeaderContainer";
import CustomMenu from "./features/menu/CustomMenu";
/* import Account from "./features/Account"; */

function App() {
  return (
    <>
      <Router>
        <Layout className="layout">
          <Header style={{
            backgroundColor: "black",

          }}>
            <CustomMenu />
          </Header>
          {/* <HeaderContainer/> */}
          <Layout style={{ minHeight: "500px" }}>
            {/* <CustomSidebar /> */}

            <Content style={{
              padding: "20px 50px",
              backgroundColor: "#7669eb"
            }}
            >
              <Routes>
                <Route path="/" element={<HomeContainer />} />
                <Route path="Auth/Login" element={<Login />} />
                <Route path="Auth/Registration" element={<Registration />} />
                {/* <Route
                  path="*"
                  element={<NotFound />}
                /> */}

                {/* <Route path="/updateAccount" element={<Account />}></Route> */}

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
