// Environment variables for dynamic configuration
const GA4_MEASUREMENT_ID =
  import.meta.env.VITE_GA4_MEASUREMENT_ID || "GTM-N895C5BN";
const SOLUTION_VERSION = import.meta.env.VITE_SOLUTION_VERSION || "VS2019";

// Google Analytics 4 Initialization
function initializeGA4() {
  window.dataLayer = window.dataLayer || [];
  function gtag() {
    dataLayer.push(arguments);
  }

  gtag("js", new Date());
  gtag("config", GA4_MEASUREMENT_ID);
}

// GA4 Event Wrapper
const GA4 = (() => {
  const sendGA4Event = (eventName, parameters) => {
    gtag("event", eventName, parameters);
  };

  return {
    showcaseRunning: () => {
      sendGA4Event("mt_showcase_running", {
        solution_version: SOLUTION_VERSION,
      });
    },
    runScenario: () => {
      console.log("mt_showcase_run_scenario");
      sendGA4Event("mt_showcase_run_scenario", {
        solution_version: SOLUTION_VERSION,
      });
    },
    pingUsButton: () => {
      console.log("mt_showcase_pingus");
      sendGA4Event("mt_showcase_pingus", {
        solution_version: SOLUTION_VERSION,
      });
    },
    createOrderEvent: () => {
      console.log("mt_showcase_createOrder");
      sendGA4Event("mt_showcase_createOrder", {
        solution_version: SOLUTION_VERSION,
      });
    },
    showcaseRetryAttempted: () => {
      console.log("mt_showcase_retry_attempted");
      sendGA4Event("mt_showcase_retry_attempted", {
        solution_version: SOLUTION_VERSION,
      });
    },
    showcaseDisplayedLicenseButton: () => {
      sendGA4Event("mt_showcase_display_get_free_license", {
        solution_version: SOLUTION_VERSION,
      });
    },
    showcaseClickedLicenseButton: () => {
      sendGA4Event("mt_showcase_license_btn_click", {
        solution_version: SOLUTION_VERSION,
      });
    },
    disableAllAdvertisingFeatures: () => {
      gtag("set", "allow_google_signals", false);
    },
  };
})();

// Kissmetrics Initialization
function initializeKissmetrics() {
  const _kmq = (window._kmq = window._kmq || []);
  const _kmk = "081ab96143b8f345362841db575656a8512960d3";

  function _kms(u) {
    setTimeout(() => {
      const d = document,
        f = d.getElementsByTagName("script")[0],
        s = d.createElement("script");
      s.type = "text/javascript";
      s.async = true;
      s.src = u;
      f.parentNode.insertBefore(s, f);
    }, 1);
  }

  _kms("//i.kissmetrics.io/i.js");
  _kms(`//scripts.kissmetrics.io/${_kmk}.2.js`);
}

// Initialize All Analytics
export function initializeAnalytics() {
  initializeGA4();
  initializeKissmetrics();

  // Example: Trigger an initial event
  GA4.quickStartRunning();
}

// Export specific event triggers
export { GA4 };
