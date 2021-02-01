"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", { value: true });
const buffer_1 = require("buffer");
class Identity {
    constructor() {
        this.claims = [];
        this.name = () => { var _a; return (_a = this.findNameClaim()[0]) === null || _a === void 0 ? void 0 : _a.val; };
        this.isAuthenicated = () => this.findNameClaim().length > 0;
        this.authenticationType = () => this.auth_typ;
        this.findNameClaim = () => this.claims.filter(c => c.typ == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
    }
}
Identity.fromBase64 = (headerValue) => headerValue ? Identity.fromJson(buffer_1.Buffer.from(headerValue, 'base64').toString('binary')) : new Identity();
Identity.fromJson = (val) => Object.assign(new Identity(), JSON.parse(val));
const httpTrigger = function (context, req) {
    return __awaiter(this, void 0, void 0, function* () {
        let identity = Identity.fromBase64(req.headers['x-ms-client-principal']);
        context.log.info(`IsAuthenticated: ${identity.isAuthenicated()}`);
        context.log.info(`Identity name: ${identity.name()}`);
        context.log.info(`AuthenticationType: ${identity.authenticationType()}`);
        identity.claims.forEach(c => context.log.info(`Claim: ${c.typ} : ${c.val}`));
        context.res = {
            // status: 200, /* Defaults to 200 */
            body: "Hello world"
        };
    });
};
exports.default = httpTrigger;
//# sourceMappingURL=index.js.map