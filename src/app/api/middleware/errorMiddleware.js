import { setErrorMessage } from "../../slice/notificationSlice";

export const errorMiddleware =
    ({ dispatch }) =>
        (next) =>
            (action) => {
                try {
                    if ((action.type).includes("rejected")) {
                        dispatch(setErrorMessage(action?.error?.message));
                    }
                } catch {
                } finally {
                    next(action);
                }
            };