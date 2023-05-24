import { configureStore } from '@reduxjs/toolkit';
import loginReducer from "./slice/authorization/loginSlice";
import authorizationReducer from "./slice/authorizationSlice";

export const store = configureStore({
  reducer: {
    login: loginReducer,
    authorization: authorizationReducer,
  },
});
