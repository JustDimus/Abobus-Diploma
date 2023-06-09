import { ROUTE_API  } from "./../addressesConst";
import axios from "axios";
import { handleError } from "../apiService";

export const createRoute = async (data) => {
    let response = await axios.post(ROUTE_API, data).catch(handleError);
    return response;
};