export const JWT = "jwt";

export const jwtService = {
    set: (value) => {
        localStorage.clear();
        localStorage.setItem(JWT, value);
    },
    
    get: () => {
        return localStorage.getItem(JWT); 
    },

    remove: () => {
        localStorage.clear();
    },

    
}