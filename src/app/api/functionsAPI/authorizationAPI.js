import { LOGIN_API, REGISTRATION_API, LOGIN_GOOGLE_API } from "./../addressesConst";
import axios from "axios";
import { jwtService } from "../../jwtService";
import { handleError } from "../apiService";

export const login = async (data) => {
    let response = await axios.post(LOGIN_API, data).catch(handleError);
    handleData(response.data);
    return response;
};

export const loginViaGoogle = async (token) => {
    debugger;
    const response = await axios.post(LOGIN_GOOGLE_API, { token: token })
        .catch(handleError);
    handleData(response.data);
    return response;
};

export const registration = async (data) => {
    let response = await axios.post(REGISTRATION_API, data).catch(handleError);
    handleData(response.data);
    return response;
};

const handleData = (data) => {
    debugger;
    jwtService.remove();
    jwtService.set(data.accessToken);
};