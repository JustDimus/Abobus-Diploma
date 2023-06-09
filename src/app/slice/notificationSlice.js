import { createSlice } from "@reduxjs/toolkit";

const initialState = {
    message: "",
    errorMessage: ""
};

export const errorSlice = createSlice({
    name: "errorSlice",
    initialState,
    reducers: {
        setMessage: (state, action) => {
            return {
                ...state,
                message: action,
            };
        },
        setErrorMessage: (state, action) => {
            return {
                ...state,
                errorMessage: action,
            };
        },
    },
});

export const { setMessage, setErrorMessage } = errorSlice.actions;
export const selectNotification = (state) => state.notification;

export default errorSlice.reducer;