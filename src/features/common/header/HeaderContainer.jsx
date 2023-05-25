import { Header } from "antd/lib/layout/layout"
import { Menu } from "antd"

const HeaderContainer = () => {
    return (
        <Header className="header_main">
            <div className="demo-logo" />
            <Menu className="header_menu"
                mode="horizontal"
                defaultSelectedKeys={['2']}
                items={new Array(3).fill(null).map((_, index) => ({
                    key: String(index + 1),
                    label: `nav ${index + 1}`,
                }))}
            />
        </Header>
    )
}


export default HeaderContainer;