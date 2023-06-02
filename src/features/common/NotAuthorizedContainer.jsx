const NotAuthorizedContainer = () => {
    return (
        <>
            <div className="page_not_found">
                <div style={{ paddingBottom: "20px", fontSize: "50px" }}>
                    Ooops
                </div>
                <div style={{ paddingBottom: "20px", fontSize: "60px" }}>
                    401
                </div>
                <div style={{ paddingBottom: "20px", fontSize: "50px" }}>
                    Unauthorized!
                </div>
            </div>

        </>
    );
}

export default NotAuthorizedContainer;