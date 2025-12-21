import "./styles/global.css";
import "./styles/main-page.css";
import "./styles/exam-type.css";

import { AppProviders } from "./providers";
import { AppRouter } from "./router";

const App = () => (
  <AppProviders>
    <AppRouter />
  </AppProviders>
);

export default App;
