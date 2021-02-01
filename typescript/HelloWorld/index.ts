import { AzureFunction, Context, HttpRequest } from "@azure/functions"
import { Buffer } from "buffer";

interface Claim {
    typ : string;
    val : string;
}

class Identity {
    claims : Claim[] = [];
    auth_typ : string;
    name_typ : string;
    role_type : string;

    name = () => this.findNameClaim()[0]?.val;
    isAuthenicated = () => this.findNameClaim().length > 0;
    authenticationType = () => this.auth_typ;

    static fromBase64 = (headerValue:string | undefined) : Identity =>
        headerValue ? Identity.fromJson(Buffer.from(headerValue, 'base64').toString('binary')) : new Identity();
    private static fromJson = (val:string) : Identity =>  Object.assign(new Identity(), JSON.parse(val));
    private findNameClaim = () => this.claims.filter(c => c.typ == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
}

const httpTrigger: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {
  
    let identity = Identity.fromBase64(req.headers['x-ms-client-principal']);
    context.log.info(`IsAuthenticated: ${identity.isAuthenicated()}`);
    context.log.info(`Identity name: ${identity.name()}`);
    context.log.info(`AuthenticationType: ${identity.authenticationType()}`);
    identity.claims.forEach(c => context.log.info(`Claim: ${c.typ} : ${c.val}`));

    context.res = {
        // status: 200, /* Defaults to 200 */
        body: "Hello world"
    };
};

export default httpTrigger;