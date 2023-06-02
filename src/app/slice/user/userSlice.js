import { loadUser, updateUser } from "../../api/functionsAPI/userAPI";
import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";

const initialState = {
  name: "",
  surname: "",
  avatar: ""
};

export const updateUserAsync = createAsyncThunk(
  "userSlice/update",
  async (form) => {
    const response = await updateUser(form);
    return response.data;
  }
);

export const loadUserAsync = createAsyncThunk(
  "userSlice/load",
  async () => {
    const response = await loadUser();
    return response.data;
  });

export const userSlice = createSlice({
  name: "user",
  initialState,
  reducers: {
    setFirstName: (state, payload) => {
      return {
        ...state,
        firstName: payload,
      };
    },
    setLastName: (state, payload) => {
      return {
        ...state,
        lastName: payload,
      };
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(updateUserAsync.fulfilled, (state, action) => {
        return action.payload;
      })
      .addCase(loadUserAsync.fulfilled, (state, action) => {
        return action.payload;
      });
  },
});

export const { setLastName, setFirstName } =
  userSlice.actions;

export const selectUser = (state) => state.user;

export default userSlice.reducer;

export const updateUserThunk =
  (img) =>
    (dispatch, getState) => {
      const formData = new FormData();
      const state = selectUser(getState());
      if (img) formData.append("image", img);

      for (let key of Object.keys(state)) {
        formData.append(key, (state)[key]);
      }

      dispatch(updateUserAsync(formData));
    };