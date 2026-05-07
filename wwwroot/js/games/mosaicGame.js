// Simplified mosaic game - tiles are now rendered by Blazor
export function revealTile(containerId, tileIndex, gridSize) {
    const container = document.getElementById(containerId);
    if (!container) {
        console.warn('Cannot reveal tile - container not found');
        return;
    }

    const tile = container.querySelector(`[data-tile-index="${tileIndex}"]`);
    if (tile) {
        tile.classList.add('revealed');
        console.log(`Revealed tile ${tileIndex}`);
    } else {
        console.warn(`Tile ${tileIndex} not found`);
    }
}

export function revealAll(containerId) {
    const container = document.getElementById(containerId);
    if (!container) {
        console.warn('Cannot reveal all - container not found');
        return;
    }

    const tiles = container.querySelectorAll('.mosaic-tile');
    console.log(`Revealing all ${tiles.length} tiles`);
    tiles.forEach(tile => {
        tile.classList.add('revealed');
    });
}
