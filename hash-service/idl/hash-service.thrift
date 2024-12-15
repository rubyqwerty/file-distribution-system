namespace netstd hash_service

enum Algorithms {
    SHA1 = 0,
    SHA256 = 1,
    MD5 = 2
}

enum Format
{
    HEX = 1,
    BASE64 = 2,
    BINARY = 3
}

struct HashParams
{
    1: string data,
    2: Algorithms algorithm,
    3: Format outputFormat,
    4: i32 hashLength,
    5: i32 numberIteration
}

service HashService
{
    string GetHash(1: HashParams params);
}