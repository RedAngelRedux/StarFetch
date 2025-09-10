export const config = {
    path: "/TMDB/*"
}

export default async (request, context) => {

    const key = Netlify.env.get("API_KEY");
    let tmdbUrl = Netlify.env.get("API_URL");

    if (!tmdbUrl.endsWith("/")) tmdbUrl += "/";

    const url = new URL(request.url);
    const tmdbPath = url.pathname.replace("/TMDB/", ""); // /tmdb/movies/popular -> movies/popular


    //// For Debugging in Production
    //console.log("Incoming request:", request.url);
    //console.log("tmdbUrl:", tmdbUrl);
    //console.log("tmdbPath:", tmdbPath);
    //console.log("Query string:", url.search);

    let targetUrl = tmdbUrl + tmdbPath + url.search

    //console.log("Netlify API Call: ", targetUrl);
    //console.log("Auth Header: ", authHeaders(key))

    try {
        const response = await fetch(targetUrl, {
            headers: authHeaders(key)
        });

        const body = await response.text();
        return corsify(body, response.status);

        //console.log("Response status:", response.status);
        //console.log("Response body: ", response.body);
        //return response;
    } catch (err) {
        console.error("Fetch failed:", err);
        return new Response("Upstream API error", { status: 502 });
    }
}

function authHeaders(token) {
    return {
        Authorization: `Bearer ${token}`
    };
}

function corsify(body, status = 200) {
    return new Response(body, {
        status,
        headers: {
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Methods": "GET, OPTIONS",
            "Access-Control-Allow-Headers": "Content-Type, Authorization"
        }
    });
}

//// The "written" way (Did not Build on Netlify As-is)
//export default {
//    // Netlify config for routing
//    path: "/TMDB/*",  // This means any request to /TMDB/ will be handled here

//    export async handler(req) {
//        const API_KEY = Deno.env.get("API_KEY");    // from Netlify environment vars
//        const API_URL = Deno.env.get("API_URL");    // e.g., https://api.themoviedb.org/3/

//        // Ensure trailing slash if missing
//        let baseUrl = API_URL.endsWith("/")
//            ? API_URL
//            : API_URL + "/";

//        // Example: the user calls /TMDB/movie/popular
//        // Remove '/TMDB' portion so we can forward to actual TMDB endpoint
//        let newUrl = req.url.replace("/TMDB/", "");

//        // Rebuild the full URL, e.g. https://api.themoviedb.org/3/movie/popular
//        let targetUrl = `${baseUrl}${newUrl}`;

//        // Add your Bearer or Query Parameter auth if needed
//        // This example might look for the "?" to attach &api_key= or Bearer tokens.

//        // Proxy the request with fetch
//        const response = await fetch(targetUrl, {
//            headers: {
//                Authorization: `Bearer ${API_KEY}`
//            },
//            method: req.method
//        });

//        return new Response(response.body, {
//            status: response.status,
//            headers: response.headers
//        });
//    }
//}
