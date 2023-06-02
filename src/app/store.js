import { configureStore } from '@reduxjs/toolkit';
import loginReducer from "./slice/authorization/loginSlice";
import authorizationReducer from "./slice/authorizationSlice";
import registrationReducer from "./slice/authorization/registrationSlice";
import { errorMiddleware } from './api/middleware/errorMiddleware';
import { authorizeMiddleware } from './api/middleware/authorizationMiddleware';
import userSlice from './slice/user/userSlice';

export const store = configureStore({
  reducer: {
    registration: registrationReducer,
    login: loginReducer,
    authorization: authorizationReducer,
    user: userSlice,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(errorMiddleware).concat(authorizeMiddleware),
});
