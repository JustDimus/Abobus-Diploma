import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';


const HomeContainer = () => {
    return (
        <>
            <div className="home_container_styling"

            >
                Aboba                
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