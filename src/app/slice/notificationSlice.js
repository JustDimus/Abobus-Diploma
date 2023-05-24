import { createSlice } from "@reduxjs/toolkit";

const initialState = {
    message: "",
    errorMessage: ""
};

export const errorSlice = createSlice({
    name: "errorSlice",
    initialState,
    reducers: {
        setMessage: (state, { payload }) => {
            return {
                ...state,
                message: payload,
            };
        },
        setErrorMessage: (state, { payload }) => {
            return {
                ...state,
                errorMessage: payload,
            };
        },
    },
});

export const { setMessage, setErrorMessage } = errorSlice.actions;
export const selectNotification = (state) => state.notification;

export default errorSlice.reducer;