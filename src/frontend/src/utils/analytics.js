const GA4 = (() => {
  // Function to send events via the dataLayer for GTM
  const sendGA4Event = (eventName, parameters = {}) => {
    if (!window.dataLayer) {
      console.error("GTM is not initialized. Unable to send GA4 event.");
      return;
    }

    window.dataLayer.push({
      event: eventName,
      ...parameters,
    });

    console.log(`GA4 event sent: ${eventName}`, parameters);
  };

  // Return an object with functions for specific events
  return {
    showcaseRunning: () => {
      sendGA4Event("mt_showcase_running");
    },
    runScenario: () => {
      console.log("mt_showcase_run_scenario");
      sendGA4Event("mt_showcase_run_scenario");
    },
    pingUsButton: () => {
      console.log("mt_showcase_pingus");
      sendGA4Event("mt_showcase_pingus");
    },
    createOrderEvent: () => {
      console.log("mt_showcase_createOrder");
      sendGA4Event("mt_showcase_createOrder");
    },
    showcaseRetryAttempted: () => {
      console.log("mt_showcase_retry_attempted");
      sendGA4Event("mt_showcase_retry_attempted");
    },
    showcaseDisplayedLicenseButton: () => {
      sendGA4Event("mt_showcase_display_get_free_license");
    },
    showcaseClickedLicenseButton: () => {
      sendGA4Event("mt_showcase_license_btn_click");
    },
    disableAllAdvertisingFeatures: () => {
      if (!window.gtag) {
        console.error("GA4 has not been initialized.");
        return;
      }
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
