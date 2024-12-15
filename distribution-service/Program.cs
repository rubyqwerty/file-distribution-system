

using chunk;
using nodes;

var chunkHandler = new MockChunk();

var chunks = chunkHandler.GetChunkNodes();

var servNodesHandler = new MockNodes();

var serverNodes = servNodesHandler.GetServerNodes();

var placer = new PlaceFounder(serverNodes, chunks, 3);

placer.ComputePlacement();

var placement = placer.Placement;

foreach (var place in placement)
{
    Console.WriteLine($"{place.chunk} --> {place.virtualServerNode}");
}