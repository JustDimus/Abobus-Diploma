import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { setAuthorization } from "../authorizationSlice";
import { registration } from "../../api/functionsAPI/authorizationAPI";

const initialState = {
    name: "",
    surname: "",
    nickname: "",
    email: "",
    password: "",
    confirmPassword: "",
};

export const registrationAsync = createAsyncThunk(
    "registrationSlice/registration",
    async (data) => {
        const response = await registration(data);
        return response.data;
    }
);

export const registrationSlice = createSlice({
    name: "registration",
    initialState,
    reducers: {
        setRegistrationName: (state, action) => {
            return {
                ...state,
                name: action.payload
            };
        },
        setRegistrationSurname: (state, action) => {
            return {
                ...state,
                surname: action.payload
            };
        },
        setRegistrationNickname: (state, action) => {
            return {
                ...state,
                nickname: action.payload
            };
        },
        setRegistrationEmail: (state, action) => {
            return {
                ...state,
                email: action.payload
            };
        },
        setRegistrationPassword: (state, action) => {
            return {
                ...state,
                password: action.payload
            };
        },
        setRegistrationConfirmPassword: (state, action) => {
            return {
                ...state,
                confirmPassword: action.payload
            };
        }
    }
});

export const {
    setRegistrationName,
    setRegistrationSurname,
    setRegistrationNickname,
    setRegistrationEmail,
    setRegistrationPassword,
    setRegistrationConfirmPassword
} = registrationSlice.actions;

export const selectRegistration = (state) => state.registration;

export default registrationSlice.reducer;

export const registrateThunk = () => (dispatch, getState) => {
    const state = selectRegistration(getState());

    dispatch(registrationAsync(state)).then((a) => {
        if (a.type.endsWith("fulfilled")) {
            dispatch(setAuthorization(true));
        }
    });
};
