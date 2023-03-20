mergeInto(LibraryManager.library, {
  SetScore: function (score) {
      try {
      window.dispatchReactUnityEvent("SetScore", score);
    } catch (e) {
      console.warn("Failed to dispatch event");
    }
  },
});