import { post } from "./../apiService";
import { USER_API } from "./../addressesConst";
import { get } from "../apiService";

export const loadUser = () => {
  return get(USER_API);
};

export const updateUser = (user) => {
  return post(USER_API, user);
};