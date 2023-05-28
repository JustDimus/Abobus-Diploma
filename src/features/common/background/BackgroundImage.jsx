import {
    useParams
} from 'react-router-dom';
import { useEffect, useState } from "react";

function BackgroundImage() {
    let params = useParams();
    let url = decodeURI(window.location.pathname);
    let [dynamicImage, setDynamicImage] = useState("");

    useEffect(() => {
        if (url == "/" || url == "/Auth/Login") {
            setDynamicImage("../public/images/main_image2.jpg")
        } else if (url == "/Auth/Registration") {
            setDynamicImage("../public/images/main_image3.jpg");
        }

    }, [url]);

    return ( 
        <div className='main_styling'
        style = {
            {
                backgroundImage: `url(${dynamicImage})`
            }
        }>
        </div>
    )
}

export default BackgroundImage;