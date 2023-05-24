import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { login, loginViaGoogle } from "../../api/functionsAPI/authorizationAPI";
import { setAuthorization } from "../authorizationSlice";

const initialState = {
    email: "",
    password: ""
}

export const loginAsync = createAsyncThunk(
    "loginSlice/login",
    async (data) => {
        const response = await login(data);
        return response.data;
    }
);

export const loginViaGoogleAsync = createAsyncThunk(
    "loginform/loginGoogle",
    async (data) => {
        const token = data.getAuthResponse().id_token;
        const response = await loginViaGoogle(token);
        return response.data;
    }
);

const loginSlice = createSlice({
    name: 'login',
    initialState,
    reducers: {
        setLoginUsername(state, action) {
            return {
                ...state,
                email: action.payload
            };
        },
        setLoginPassword(stat, action) {
            return {
                ...state,
                password: action.payload
            };
        },
    },
    extraReducers(builder) {
        builder.addCase(loginAsync.fulfilled, (state, action) => {
            return {
                ...initialState
            };
        });
    },
})

export const loginThunk = () => async (dispatch, getState) => {
    const state = selectLogin(getState());
    try {
      const response = await dispatch(loginAsync(state));
      if (response.type.endsWith("fulfilled")) {
        debugger;
        dispatch(setAuthorization(true));
      }
    } catch (error) {
      throw error;
    }
  };

export const { setLoginUsername, setLoginPassword } = loginSlice.actions
export const selectLogin = (state) => state.login;

export default loginSlice.reducer