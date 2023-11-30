import __vite__cjsImport0_react_jsxDevRuntime from "/node_modules/.vite/deps/react_jsx-dev-runtime.js?v=71a749d3"; const jsxDEV = __vite__cjsImport0_react_jsxDevRuntime["jsxDEV"];
import __vite__cjsImport1_react from "/node_modules/.vite/deps/react.js?v=71a749d3"; const React = __vite__cjsImport1_react.__esModule ? __vite__cjsImport1_react.default : __vite__cjsImport1_react;
import __vite__cjsImport2_reactDom_client from "/node_modules/.vite/deps/react-dom_client.js?v=71a749d3"; const ReactDOM = __vite__cjsImport2_reactDom_client.__esModule ? __vite__cjsImport2_reactDom_client.default : __vite__cjsImport2_reactDom_client;
import App from "/src/App.tsx?t=1701330596938";
import "/src/index.css?t=1701327633252";
import { AppContextProvider } from "/src/context/AppContext.tsx";
import {
  createBrowserRouter,
  RouterProvider
} from "/node_modules/.vite/deps/react-router-dom.js?v=71a749d3";
import Registeration from "/src/views/registration.tsx?t=1701327602280";
import PasswordRecovery from "/src/views/password_recovery.tsx?t=1701330596938";
import Login from "/src/views/Login.tsx?t=1701330596938";
import CompletePasswordRecovery from "/src/views/complete_password_recovery.tsx?t=1701330596938";
import Landing from "/src/views/landing.tsx?t=1701330596938";
const router = createBrowserRouter(
  [
    {
      path: "/",
      element: /* @__PURE__ */ jsxDEV(Landing, {}, void 0, false, {
        fileName: "/Users/aldanisvigo/Projects/FamilyCalendarDotNet/frontend/src/main.tsx",
        lineNumber: 18,
        columnNumber: 12
      }, this)
    },
    {
      path: "/register",
      element: /* @__PURE__ */ jsxDEV(Registeration, {}, void 0, false, {
        fileName: "/Users/aldanisvigo/Projects/FamilyCalendarDotNet/frontend/src/main.tsx",
        lineNumber: 22,
        columnNumber: 12
      }, this)
    },
    {
      path: "/recover",
      element: /* @__PURE__ */ jsxDEV(PasswordRecovery, {}, void 0, false, {
        fileName: "/Users/aldanisvigo/Projects/FamilyCalendarDotNet/frontend/src/main.tsx",
        lineNumber: 26,
        columnNumber: 12
      }, this)
    },
    {
      path: "/complete_recovery",
      element: /* @__PURE__ */ jsxDEV(CompletePasswordRecovery, {}, void 0, false, {
        fileName: "/Users/aldanisvigo/Projects/FamilyCalendarDotNet/frontend/src/main.tsx",
        lineNumber: 30,
        columnNumber: 12
      }, this)
    },
    {
      path: "/home",
      element: /* @__PURE__ */ jsxDEV(App, {}, void 0, false, {
        fileName: "/Users/aldanisvigo/Projects/FamilyCalendarDotNet/frontend/src/main.tsx",
        lineNumber: 34,
        columnNumber: 12
      }, this)
    },
    {
      path: "/login",
      element: /* @__PURE__ */ jsxDEV(Login, {}, void 0, false, {
        fileName: "/Users/aldanisvigo/Projects/FamilyCalendarDotNet/frontend/src/main.tsx",
        lineNumber: 38,
        columnNumber: 12
      }, this)
    }
  ]
);
ReactDOM.createRoot(document.getElementById("root")).render(
  /* @__PURE__ */ jsxDEV(React.StrictMode, { children: /* @__PURE__ */ jsxDEV(AppContextProvider, { children: /* @__PURE__ */ jsxDEV(RouterProvider, { router }, void 0, false, {
    fileName: "/Users/aldanisvigo/Projects/FamilyCalendarDotNet/frontend/src/main.tsx",
    lineNumber: 45,
    columnNumber: 7
  }, this) }, void 0, false, {
    fileName: "/Users/aldanisvigo/Projects/FamilyCalendarDotNet/frontend/src/main.tsx",
    lineNumber: 44,
    columnNumber: 5
  }, this) }, void 0, false, {
    fileName: "/Users/aldanisvigo/Projects/FamilyCalendarDotNet/frontend/src/main.tsx",
    lineNumber: 43,
    columnNumber: 3
  }, this)
);

//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJtYXBwaW5ncyI6IkFBaUJhO0FBakJiLE9BQU9BLFdBQVc7QUFDbEIsT0FBT0MsY0FBYztBQUNyQixPQUFPQyxTQUFTO0FBQ2hCLE9BQU87QUFDUCxTQUFTQywwQkFBMEI7QUFDbkM7QUFBQSxFQUFTQztBQUFBQSxFQUNQQztBQUFBQSxPQUNLO0FBQ1AsT0FBT0MsbUJBQW1CO0FBQzFCLE9BQU9DLHNCQUFzQjtBQUM3QixPQUFPQyxXQUFXO0FBQ2xCLE9BQU9DLDhCQUE4QjtBQUNyQyxPQUFPQyxhQUFhO0FBRXBCLE1BQU1DLFNBQVNQO0FBQUFBLEVBQW9CO0FBQUEsSUFDakM7QUFBQSxNQUNFUSxNQUFNO0FBQUEsTUFDTkMsU0FBUyx1QkFBQyxhQUFEO0FBQUE7QUFBQTtBQUFBO0FBQUEsYUFBUTtBQUFBLElBQ25CO0FBQUEsSUFDQTtBQUFBLE1BQ0VELE1BQU87QUFBQSxNQUNQQyxTQUFTLHVCQUFDLG1CQUFEO0FBQUE7QUFBQTtBQUFBO0FBQUEsYUFBYztBQUFBLElBQ3pCO0FBQUEsSUFDQTtBQUFBLE1BQ0VELE1BQU87QUFBQSxNQUNQQyxTQUFVLHVCQUFDLHNCQUFEO0FBQUE7QUFBQTtBQUFBO0FBQUEsYUFBaUI7QUFBQSxJQUM3QjtBQUFBLElBQ0E7QUFBQSxNQUNFRCxNQUFPO0FBQUEsTUFDUEMsU0FBVSx1QkFBQyw4QkFBRDtBQUFBO0FBQUE7QUFBQTtBQUFBLGFBQXlCO0FBQUEsSUFDckM7QUFBQSxJQUNBO0FBQUEsTUFDRUQsTUFBTztBQUFBLE1BQ1BDLFNBQVUsdUJBQUMsU0FBRDtBQUFBO0FBQUE7QUFBQTtBQUFBLGFBQUk7QUFBQSxJQUNoQjtBQUFBLElBQ0E7QUFBQSxNQUNFRCxNQUFPO0FBQUEsTUFDUEMsU0FBUyx1QkFBQyxXQUFEO0FBQUE7QUFBQTtBQUFBO0FBQUEsYUFBTTtBQUFBLElBQ2pCO0FBQUEsRUFBQztBQUNGO0FBRURaLFNBQVNhLFdBQVdDLFNBQVNDLGVBQWUsTUFBTSxDQUFFLEVBQUVDO0FBQUFBLEVBQ3BELHVCQUFDLE1BQU0sWUFBTixFQUNDLGlDQUFDLHNCQUNDLGlDQUFDLGtCQUFlLFVBQWhCO0FBQUE7QUFBQTtBQUFBO0FBQUEsU0FBK0IsS0FEakM7QUFBQTtBQUFBO0FBQUE7QUFBQSxTQUVBLEtBSEY7QUFBQTtBQUFBO0FBQUE7QUFBQSxTQUlBO0FBQ0YiLCJuYW1lcyI6WyJSZWFjdCIsIlJlYWN0RE9NIiwiQXBwIiwiQXBwQ29udGV4dFByb3ZpZGVyIiwiY3JlYXRlQnJvd3NlclJvdXRlciIsIlJvdXRlclByb3ZpZGVyIiwiUmVnaXN0ZXJhdGlvbiIsIlBhc3N3b3JkUmVjb3ZlcnkiLCJMb2dpbiIsIkNvbXBsZXRlUGFzc3dvcmRSZWNvdmVyeSIsIkxhbmRpbmciLCJyb3V0ZXIiLCJwYXRoIiwiZWxlbWVudCIsImNyZWF0ZVJvb3QiLCJkb2N1bWVudCIsImdldEVsZW1lbnRCeUlkIiwicmVuZGVyIl0sInNvdXJjZXMiOlsibWFpbi50c3giXSwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IFJlYWN0IGZyb20gJ3JlYWN0J1xuaW1wb3J0IFJlYWN0RE9NIGZyb20gJ3JlYWN0LWRvbS9jbGllbnQnXG5pbXBvcnQgQXBwIGZyb20gJy4vQXBwLnRzeCdcbmltcG9ydCAnLi9pbmRleC5jc3MnXG5pbXBvcnQgeyBBcHBDb250ZXh0UHJvdmlkZXIgfSBmcm9tICcuL2NvbnRleHQvQXBwQ29udGV4dCdcbmltcG9ydCB7IGNyZWF0ZUJyb3dzZXJSb3V0ZXIsXG4gIFJvdXRlclByb3ZpZGVyLFxufSBmcm9tIFwicmVhY3Qtcm91dGVyLWRvbVwiO1xuaW1wb3J0IFJlZ2lzdGVyYXRpb24gZnJvbSAnLi92aWV3cy9yZWdpc3RyYXRpb24udHN4J1xuaW1wb3J0IFBhc3N3b3JkUmVjb3ZlcnkgZnJvbSAnLi92aWV3cy9wYXNzd29yZF9yZWNvdmVyeS50c3gnXG5pbXBvcnQgTG9naW4gZnJvbSAnLi92aWV3cy9Mb2dpbi50c3gnXG5pbXBvcnQgQ29tcGxldGVQYXNzd29yZFJlY292ZXJ5IGZyb20gJy4vdmlld3MvY29tcGxldGVfcGFzc3dvcmRfcmVjb3ZlcnkudHN4J1xuaW1wb3J0IExhbmRpbmcgZnJvbSAnLi92aWV3cy9sYW5kaW5nLnRzeCdcblxuY29uc3Qgcm91dGVyID0gY3JlYXRlQnJvd3NlclJvdXRlcihbXG4gIHtcbiAgICBwYXRoOiBcIi9cIixcbiAgICBlbGVtZW50OiA8TGFuZGluZy8+LFxuICB9LFxuICB7XG4gICAgcGF0aCA6IFwiL3JlZ2lzdGVyXCIsXG4gICAgZWxlbWVudDogPFJlZ2lzdGVyYXRpb24gLz5cbiAgfSxcbiAge1xuICAgIHBhdGggOiBcIi9yZWNvdmVyXCIsXG4gICAgZWxlbWVudCA6IDxQYXNzd29yZFJlY292ZXJ5Lz5cbiAgfSxcbiAge1xuICAgIHBhdGggOiAnL2NvbXBsZXRlX3JlY292ZXJ5JyxcbiAgICBlbGVtZW50IDogPENvbXBsZXRlUGFzc3dvcmRSZWNvdmVyeS8+XG4gIH0sXG4gIHtcbiAgICBwYXRoIDogJy9ob21lJyxcbiAgICBlbGVtZW50IDogPEFwcC8+XG4gIH0sXG4gIHtcbiAgICBwYXRoIDogJy9sb2dpbicsXG4gICAgZWxlbWVudDogPExvZ2luLz5cbiAgfVxuXSk7XG5cblJlYWN0RE9NLmNyZWF0ZVJvb3QoZG9jdW1lbnQuZ2V0RWxlbWVudEJ5SWQoJ3Jvb3QnKSEpLnJlbmRlcihcbiAgPFJlYWN0LlN0cmljdE1vZGU+XG4gICAgPEFwcENvbnRleHRQcm92aWRlcj5cbiAgICAgIDxSb3V0ZXJQcm92aWRlciByb3V0ZXI9e3JvdXRlcn0gLz5cbiAgICA8L0FwcENvbnRleHRQcm92aWRlcj5cbiAgPC9SZWFjdC5TdHJpY3RNb2RlPixcbilcbiJdLCJmaWxlIjoiL1VzZXJzL2FsZGFuaXN2aWdvL1Byb2plY3RzL0ZhbWlseUNhbGVuZGFyRG90TmV0L2Zyb250ZW5kL3NyYy9tYWluLnRzeCJ9