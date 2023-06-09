import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { createRoute } from "../../api/functionsAPI/mapAPI";

/* const route = {
    startAddress: "",
    endAddress: "",
    startLocation: {},
    endLocation: {},
    distance: 0,
    duration: 0,
    geocodedWaypoints: {},
}; */

const initialState = {
    routes: [],
};

export const createRouteAsync = createAsyncThunk(
    "routeSlice/route",
    async (data) => {
        const response = await createRoute(data);
        return response.data;
    }
);

const routeSlice = createSlice({
    name: 'route',
    initialState,
    reducers: {
        setRoute: (state, action) => {
            state.routes.push(action.payload);
        },
        deletePreviousRoute: (state) => {
            if (state.routes.length > 0) {
                state.routes.pop();
            }
            return;
        },
        cleanRouteData: (state) => {
            return {
                ...initialState
            };
        },
    },
    extraReducers(builder) {
        builder.addCase(createRouteAsync.fulfilled, (state, action) => {
            return {
                ...initialState
            };
        });
    },
});

/* const routeSlice = createSlice({
    name: 'route',
    initialState,
    reducers: {
        setRoute: (state, action) => {
            const identicalRoute = state.routes.find((route) => route.startAddress === action.payload.startAddress
                && route.endAddress === action.payload.endAddress);

            if (identicalRoute) {
                return
            }
            else {
                state.routes.push(action.payload);
            }
        },
        deletePreviousRoute: (state) => {
            if (state.routes.length > 0) {
                state.routes.pop();
            }
            return;
        },
        cleanRouteData: (state) => {
            return {
                ...initialState
            };
        },
    },
    extraReducers(builder) {
        builder.addCase(createRouteAsync.fulfilled, (state, action) => {
            return {
                ...initialState
            };
        });
    },
}); */

export const { setRoute, deletePreviousRoute, cleanRouteData } = routeSlice.actions;

export default routeSlice.reducer;