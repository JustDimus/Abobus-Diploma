import { createSlice } from "@reduxjs/toolkit";

const initialState = {
    isAuthorize: false,
};

export const authorizationSlice = createSlice({
    name: "authorization",
    initialState,
    reducers: {
        setAuthorization: (state, action) => {
            return {
                ...state,
                isAuthorize: action.payload
            };
        },
        resetUser: () => {
            return {
                ...initialState,
            };
        },
    },
});

export const { setAuthorization, resetUser } = authorizationSlice.actions;
export const selectAuthorization = (state) => state.authorization;

export default authorizationSlice.reducer;