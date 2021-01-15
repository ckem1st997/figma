"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.$ = exports.r = void 0;
/**
 * Convert a list to an array.
 *
 * @param 	{NodeList|HTMLCollection} list 	The list or collection to convert into an array.
 * @return	{array}							The array.
 */
exports.r = function (list) {
    return Array.prototype.slice.call(list);
};
/**
 * Find elements in the given context.
 *
 * @param 	{string}		selector			The query selector to search for.
 * @param 	{HTMLElement}	[context=document]	The context to search in.
 * @return	{HTMLElement[]}						The found list of elements.
 */
exports.$ = function (selector, context) {
    return exports.r((context || document).querySelectorAll(selector));
};
//# sourceMappingURL=helpers.js.map