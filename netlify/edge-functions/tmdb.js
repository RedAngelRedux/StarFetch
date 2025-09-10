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

    let targetUrl = tmdbUrl + tmdbPath + url.search

    try {
        const response = await fetch(targetUrl, {
            headers: authHeaders(key)
        });

        const body = await response.text();
        return corsify(body, response.status);

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