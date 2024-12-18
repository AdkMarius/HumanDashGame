using System;

namespace HumanDash.Exception;

public class IncorrectInput(String message) : global::System.Exception(message);