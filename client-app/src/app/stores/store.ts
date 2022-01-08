import BookStore from "./bookStore";
import {createContext, useContext} from "react";
import UserStore from "./userStore";
import CommonStore from "./commonStore";

interface Store {
    bookStore: BookStore,
    commonStore: CommonStore;
    userStore: UserStore;
}

export const store: Store = {
    bookStore: new BookStore(),
    commonStore: new CommonStore(),
    userStore: new UserStore(),
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}
