"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.IE11 = exports.touch = void 0;
/** Whether or not touch gestures are supported by the browser. */
exports.touch = 'ontouchstart' in window ||
    (navigator.msMaxTouchPoints ? true : false) ||
    false;
/** Whether or not its IE11 :/ */
exports.IE11 = navigator.userAgent.indexOf('MSIE') > -1 ||
    navigator.appVersion.indexOf('Trident/') > -1;
//# sourceMappingURL=support.js.map