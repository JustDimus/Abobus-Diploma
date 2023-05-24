import { jwtService } from "../../jwtService";
import { setAuthorization } from "../../slice/authorizationSlice";

export const authorizeMiddleware =
    ({ dispatch }) =>
        (next) =>
            (action) => {
                try {
                    if (!jwtService.get() && action.type !== setAuthorization.type) {
                        dispatch(setAuthorization(false));
                    }
                } catch {
                } finally {
                    next(action);
                }
            };