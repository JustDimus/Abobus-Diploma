import axios from "axios";
import { jwtService } from "./../jwtService";

export const get = async (url) => {
    return await axios
        .get(url, {
            headers: { Authorization: "Bearer " + jwtService.get() },
        })
        .catch(handleError);
};

export const getWithParams = async (url, data) => {
    return await axios
        .get(url, {
            headers: { Authorization: "Bearer " + jwtService.get() },
            params: {
                filter: data.filter,
                name: data.name,
                page: data.page,
                sort: data.sortType,

            }
        })
}

export const post = (url, data) => {
    debugger;
    return axios
        .post(url, data, {
            headers: { Authorization: "Bearer " + jwtService.get() },
        })
        .catch(handleError);
};

export const delet = (url) => {
    return axios
        .delete(url, {
            headers: { Authorization: "Bearer " + jwtService.get() },
        })
        .catch(handleError);
};

export const put = (url, data) => {
    return axios
        .put(url, data, {
            headers: { Authorization: "Bearer " + jwtService.get() },
        })
        .catch(handleError);
};

export const handleError = (error) => {
    if (error.response.status === 401) {
        jwtService.remove();
    }
    console.log(error.response.data);
    debugger;
    throw new Error(
        error.response.data.Message ||
        error.response.data.message ||
        Object.values(error.response.data.errors)[0]
    );
};