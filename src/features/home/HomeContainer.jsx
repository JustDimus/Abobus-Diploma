import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Button } from 'antd';
import googleplay from "../../images/google-play.svg";
import appstore from "../../images/app-store.png";
import main_image from "../../images/main_image4.jpg"


const HomeContainer = () => {
    return (
        <>
            <div
                style={{
                    padding: "1em",
                    fontSize: "40px",
                    textAlign: "center",
                    background: "#2D253A",
                    fontFamily: "Lobster",
                    color: "white"
                }}
            >
                Find and get to new places that might be your favorite
            </div>
            <div
                style={{
                    display: "flex",
                    justifyContent: "space-around",
                    padding: "2em",
                    background: "#2D253A",
                }}
            >
                <Button type='primary'
                    style={{
                        borderRadius: "10px",
                        color: "white",
                        width: "200px",
                        height: "50px",
                        fontSize: "20px",
                        cursor: "pointer",
                        fontFamily: "Lobster"
                    }}

                >
                    Create a trip
                </Button>
                <p style={{ fontSize: "20px", color: "black", fontWeight: 500, fontFamily: "Mulish", color: "white" }}>
                    Don't forget to logged in!
                </p>
            </div>
            <div
                style={{
                    backgroundColor: "#00BFFF",
                    padding: "2em",
                    fontSize: "30px",
                    background: "#B07090"
                }}
            >
                <p style={{ textAlign: "center", fontFamily: "Lobster" }}>
                    Even more convenient in the Abobus mobile application
                </p>
                <div style={{ display: "flex", justifyContent: "space-around" }}>
                    <div>
                        <div style={{ paddingLeft: "2em", padding: "1em", display: "flex" }}>
                            <img
                                src={appstore}
                                style={{ height: "50px", width: "50px" }}
                            ></img>{" "}
                            <a
                                href="#"
                                style={{
                                    paddingLeft: "10px",
                                    textDecoration: "none",
                                    color: "black",
                                    fontFamily: "Mulish"
                                }}
                            >
                                Click to download
                            </a>
                        </div>
                        <div style={{ paddingLeft: "2em", padding: "1em", display: "flex" }}>
                            <img
                                src={googleplay}
                                style={{ height: "50px", width: "50px" }}
                            ></img>{" "}
                            <a
                                href="#"
                                style={{
                                    paddingLeft: "10px",
                                    textDecoration: "none",
                                    color: "black",
                                    fontFamily: "Mulish"
                                }}
                            >
                                Click to download
                            </a>
                        </div>
                    </div>
                    <img
                        src={"https://images.unsplash.com/photo-1594908562917-9a691d054b8e?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8OXx8NjAwMHg0MDAwfGVufDB8fDB8fHww&auto=format&fit=crop&w=500&q=60"}
                        style={{
                            height: "600px",
                            width: "400px",
                            transform: "rotate(165deg)",
                            boxShadow: "10px 10px 5px #black",
                        }}
                    ></img>
                </div>
            </div>
            <div style={{ padding: "3em", background: "#81476D" }}>
                <p style={{ fontSize: "35px", fontFamily: "Lobster", paddingBottom:"20px" }}>Reviews about our application</p>
                <p style={{ color: "lightgray", fontSize: "30px", fontFamily: "Mulish", paddingBottom: "10px" }}>
                    100,500 reviews in the last 12 months.
                </p>
                <div style={{ display: "flex", justifyContent: "space-around" }}>
                    <div
                        style={{
                            background: "#9966CC",
                            height: "450px",
                            width: "400px",
                        }}
                    >
                        <div style={{ display: "flex", flexDirection: "row" }}>
                            <img
                                src={"https://play-lh.googleusercontent.com/UjaAdTYsArv7zAJbqGWjQw2ftuOtnAlvokffC3TQQ2K12mwk0YdXUF2wZBTBA2kDZIk"}
                                style={{ padding: "2em", height: "150px", width: "150px", borderRadius: "50%" }}
                            ></img>
                            <div style={{ display: "flex", flexDirection: "column" }}>
                                <p
                                    style={{
                                        paddingTop: "2em",
                                        fontSize: "17px",
                                    }}
                                >
                                    Emily Harrison
                                </p>

                            </div>
                        </div>
                        <div
                            style={{
                                display: "flex",
                                flexDirection: "row",
                                paddingLeft: "2em",
                            }}
                        >
                            <p style={{ color: "yellow", fontSize: "24px" }}>★</p>
                            <p style={{ color: "yellow", fontSize: "24px" }}>★</p>
                            <p style={{ color: "yellow", fontSize: "24px" }}>★</p>
                            <p style={{ color: "yellow", fontSize: "24px" }}>★</p>
                            <p style={{ color: "yellow", fontSize: "24px" }}>★</p>
                        </div>
                        <div style={{ textAlign: "center", paddingBottom: "10px" }}>
                            <p>
                                From the moment I downloaded it, I was taken aback by its intuitive interface and the clarity of its instructions.
                                The design is both aesthetically pleasing and highly functional, allowing me to navigate with ease.
                                The feature that particularly stands out is the real-time traffic updates. They have saved me from countless traffic jams,
                                helping me reach my destinations more efficiently. The suggestions for alternative routes are accurate and have proven to be faster on multiple occasions.
                            </p>
                        </div>
                        <div
                            style={{
                                display: "flex",
                                flexDirection: "row",
                                paddingLeft: "1em",
                            }}
                        >
                            <p style={{ paddingRight: "1em" }}>July 20, 2022</p>
                            <p style={{ paddingRight: "1em" }}>·</p>
                            <p style={{ paddingRight: "1em" }}>New-York</p>
                        </div>

                    </div>
                    <div
                        style={{ background: "#9966CC", height: "450px", width: "400px" }}
                    >
                        <div style={{ display: "flex", flexDirection: "row" }}>
                            <img
                                src={"https://images.unsplash.com/photo-1535713875002-d1d0cf377fde?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NHx8bWFsZSUyMHByb2ZpbGV8ZW58MHx8MHx8fDA%3D&w=1000&q=80"}
                                style={{ padding: "2em", height: "150px", width: "150px", borderRadius: "50%" }}
                            ></img>
                            <div style={{ display: "flex", flexDirection: "column" }}>
                                <p
                                    style={{
                                        paddingTop: "2em",
                                        fontSize: "17px",
                                    }}
                                >
                                    Alexander Thornton
                                </p>

                            </div>
                        </div>
                        <div
                            style={{
                                display: "flex",
                                flexDirection: "row",
                                paddingLeft: "2em",
                            }}
                        >
                            <p style={{ color: "yellow", fontSize: "24px" }}>★</p>
                            <p style={{ color: "yellow", fontSize: "24px" }}>★</p>
                            <p style={{ color: "yellow", fontSize: "24px" }}>★</p>
                            <p style={{ color: "yellow", fontSize: "24px" }}>★</p>
                            <p style={{ fontSize: "24px" }}>★</p>
                        </div>
                        <div style={{ textAlign: "center", paddingBottom: "10px", paddingLeft:"2px" }}>
                            <p>
                                From the moment I downloaded it, I was taken aback by its intuitive interface and the clarity of its instructions.
                                The design is both aesthetically pleasing and highly functional, allowing me to navigate with ease.
                                The feature that particularly stands out is the real-time traffic updates. They have saved me from countless traffic jams,
                                helping me reach my destinations more efficiently. The suggestions for alternative routes are accurate and have proven to be faster on multiple occasions.
                            </p>
                        </div>
                        <div
                            style={{
                                display: "flex",
                                flexDirection: "row",
                                paddingLeft: "1em",
                            }}
                        >
                            <p style={{ paddingRight: "1em" }}>December 18, 2022</p>
                            <p style={{ paddingRight: "1em" }}>·</p>
                            <p style={{ paddingRight: "1em" }}>London</p>
                        </div>
                    </div>
                    <div
                        style={{ background: "#9966CC", height: "450px", width: "400px" }}
                    >
                        <div style={{ display: "flex", flexDirection: "row" }}>
                            <img
                                src={"https://sourcenm.com/wp-content/uploads/2021/08/ProfileSquarePatSOURCE.jpg"}
                                style={{ padding: "2em", height: "150px", width: "150px", backgroundSize: "cover", borderRadius: "50%" }}
                            ></img>
                            <div style={{ display: "flex", flexDirection: "column" }}>
                                <p
                                    style={{
                                        paddingTop: "2em",
                                        fontSize: "17px",
                                    }}
                                >
                                    Jonathan Sinclair
                                </p>

                            </div>
                        </div>
                        <div
                            style={{
                                display: "flex",
                                flexDirection: "row",
                                paddingLeft: "2em",
                            }}
                        >
                            <p style={{ color: "yellow", fontSize: "24px" }}>★</p>
                            <p style={{ color: "yellow", fontSize: "24px" }}>★</p>
                            <p style={{ color: "yellow", fontSize: "24px" }}>★</p>
                            <p style={{ color: "yellow", fontSize: "24px" }}>★</p>
                            <p style={{ color: "yellow", fontSize: "24px" }}>★</p>
                        </div>
                        <div style={{ textAlign: "center", paddingBottom: "10px" }}>
                            <p>
                                From the moment I downloaded it, I was taken aback by its intuitive interface and the clarity of its instructions.
                                The design is both aesthetically pleasing and highly functional, allowing me to navigate with ease.
                                The feature that particularly stands out is the real-time traffic updates. They have saved me from countless traffic jams,
                                helping me reach my destinations more efficiently. The suggestions for alternative routes are accurate and have proven to be faster on multiple occasions.
                            </p>
                        </div>
                        <div
                            style={{
                                display: "flex",
                                flexDirection: "row",
                                paddingLeft: "1em",
                            }}
                        >
                            <p style={{ paddingRight: "1em" }}>March 5, 2023</p>
                            <p style={{ paddingRight: "1em" }}>·</p>
                            <p style={{ paddingRight: "1em" }}>Berlin</p>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}

const mapStateToProps = ({
}) => {
    return {
    };
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators(
        {
        },
        dispatch
    );
};

export default connect(mapStateToProps, mapDispatchToProps)(HomeContainer)