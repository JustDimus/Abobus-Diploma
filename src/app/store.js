import { configureStore } from '@reduxjs/toolkit';
import loginReducer from "./slice/authorization/loginSlice";
import authorizationReducer from "./slice/authorizationSlice";
import registrationReducer from "./slice/authorization/registrationSlice";
import { errorMiddleware } from './api/middleware/errorMiddleware';
import { authorizeMiddleware } from './api/middleware/authorizationMiddleware';

export const store = configureStore({
  reducer: {
    registration: registrationReducer,
    login: loginReducer,
    authorization: authorizationReducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(errorMiddleware).concat(authorizeMiddleware),
});
