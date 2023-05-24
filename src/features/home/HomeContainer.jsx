import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';


const HomeContainer = () => {
    return (
        <>
            <div
                style={{
                    padding: "1em",
                    fontSize: "40px",
                    textAlign: "center",
                    border: "3px inset #00BFFF",
                    borderRadius: "30px",
                }}
            >
                The best library services for you
            </div>
            <div
                style={{
                    display: "flex",
                    justifyContent: "space-around",
                    padding: "2em",
                }}
            >
                <button
                    style={{
                        backgroundColor: "#1B98F5",
                        borderRadius: "10px",
                        padding: "1em",
                        color: "white",
                        fontWeight: 700,
                        paddingLeft: "2em",
                        paddingRight: "2em",
                        fontSize: "16px",
                        cursor: "pointer",
                    }}
                >
                    Find book
                </button>
                <p style={{ fontSize: "20px", color: "black", fontWeight: 500 }}>
                    12345
                </p>
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