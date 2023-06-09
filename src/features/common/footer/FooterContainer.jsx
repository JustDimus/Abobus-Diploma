import {
    Box,
    Container,
    Row,
    Column,
    Heading,
    FooterLink,
} from "./FooterStyle";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome"
import { faFacebook, faInstagram, faYoutube, faTwitter } from "@fortawesome/free-brands-svg-icons"

const FooterContainer = () => {

    return (
        <Box>
            <h1 style={{ color: "white", textAlign: "center", marginTop: "-50px", fontSize:"24px", paddingBottom:"20px" }}>
                Find and get to new places that might be your favorite
            </h1>
            <Container>
                <Row>
                    <Column>
                        <Heading>About Us</Heading>
                        <FooterLink href="#">Aim</FooterLink>
                        <FooterLink href="#">Vision</FooterLink>
                    </Column>
                    <Column>
                        <Heading>Services</Heading>
                        <FooterLink href="Account/Map">Map</FooterLink>
                        <FooterLink href="Account/History">History</FooterLink>
                    </Column>
                    <Column>
                        <Heading>Contact Us</Heading>
                        <FooterLink href="#">JustDimus</FooterLink>
                        <FooterLink href="#">NoobBogdan</FooterLink>
                        <FooterLink href="#">HatinAnton</FooterLink>
                        <FooterLink href="#">Yaroslavl</FooterLink>
                    </Column>
                    <Column>
                        <Heading>Social Media</Heading>
                        <FooterLink href="#">
                            <i>
                                <FontAwesomeIcon icon={faFacebook} />
                                <span style={{ marginLeft: "10px" }}>Facebook</span>
                            </i>
                        </FooterLink>
                        <FooterLink href="#">
                            <i>
                                <FontAwesomeIcon icon={faInstagram} />
                                <span style={{ marginLeft: "10px" }}>Instagram</span>
                            </i>
                        </FooterLink>
                        <FooterLink href="#">
                            <i>
                                <FontAwesomeIcon icon={faTwitter} />
                                <span style={{ marginLeft: "10px" }}>Twitter</span>
                            </i>
                        </FooterLink>
                        <FooterLink href="#">
                            <i>
                                <FontAwesomeIcon icon={faYoutube} />
                                <span style={{ marginLeft: "10px" }}>Youtube</span>
                            </i>
                        </FooterLink>
                    </Column>
                </Row>
            </Container>
        </Box>
    );
}

export default FooterContainer;