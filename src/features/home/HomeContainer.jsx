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